using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Business.Services.LimsService;
using App.Business.ViewModels;
using App.Core.Data.CustomAudit;
using App.Core.Data.Entities.Common;
using App.Core.Security.Entities;
using App.Data.Contexts;
using App.Data.Models;
using App.Data.Models.ORG;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace App.Business.Services.Token
{
    public class TokenService: ITokenService
    {
        private readonly IConfiguration config;
        private readonly MigrationDbContext _context;
        private readonly LimsExchangeService _limsService;

        public TokenService(IConfiguration configuration, MigrationDbContext context, LimsExchangeService limsService)
        {
            config = configuration;
            _context = context;
            _limsService = limsService;
        }

        public async Task<TokenInfo> GetIdGovUaToken(string code)
        {
            var authUri = config["IdGovUaProvider:Url"];
            var redirectUri = config["IdGovUaProvider:RedirectUri"];
            var clientId = config["IdGovUaProvider:ClientId"];
            var clientSecret = config["IdGovUaProvider:ClientSecret"];

            var values = new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"code", code},
                {"redirect_uri", $"{redirectUri}/home"}
            };

            var content = new FormUrlEncodedContent(values);

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(authUri);
                    client.DefaultRequestHeaders.Clear();
                    var response = await client.PostAsync($"{authUri}/get-access-token", content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(resp);

                        return tokenInfo;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Console.Write($"Error getting token from id.gov.ua {ex.Message}");
                    Log.Debug(ex.Message, "ClientToken getting error");
                }
            }

            return null;
        }

        public async Task<IdGovUaUserInfo> GetIdGovUaUserInfo(TokenInfo tokenModel)
        {
            var authUri = config["IdGovUaProvider:Url"];
            var values = new Dictionary<string, string>
            {
                {"access_token", tokenModel.access_token},
                {"user_id", tokenModel.user_id}
            };
            var content = new FormUrlEncodedContent(values);

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(authUri);
                    client.DefaultRequestHeaders.Clear();
                    var response = await client.PostAsync($"{authUri}/get-user-info", content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        var test = JsonConvert.DeserializeObject<IdGovUaUserInfo>(resp);

                        return test;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Console.Write($"Error getting user info from id.gov.ua {ex.Message}");
                    Log.Debug(ex.Message, "Error getting user info from id.gov.ua");
                }
            }

            return null;
        }

        public async Task<(string organizationId, Guid employeeId, Guid profileId, Guid personId)> CheckOrgEmployeeUnit(
            IdGovUaUserInfo userInfo)
        {
            var orgEmployee = await _context.EmployeesExt.Where(x =>
                    true && x.Person.IPN == userInfo.drfocode && x.Person.LastName == userInfo.lastname)
                .ToListAsync();
            var profile = _context.Profiles.FirstOrDefault(x => x.Caption == "Employee");
            if (!string.IsNullOrEmpty(userInfo.edrpoucode))
            {
                var organization =
                    await _context.Organizations.FirstOrDefaultAsync(x => x.EDRPOU == userInfo.edrpoucode);
                if (organization == null)
                {
                    throw new Exception();
                }

                if (orgEmployee == null)
                {
                    throw new Exception();
                }

                var employee = orgEmployee.FirstOrDefault(x => x.OrganizationId == organization.Id);
                if (employee != null)
                {
                    return (organization.Id.ToString(), employee.Id, profile.Id, employee.PersonId);
                }

                throw new Exception();
            }

            if (orgEmployee == null)
            {
                throw new Exception();
            }

            if (orgEmployee.All(x => x.OrganizationId == null))
            {
                throw new Exception();
            }

            {
                var organization = await _context.Organizations.FirstOrDefaultAsync(x =>
                    orgEmployee.Select(y => y.OrganizationId).Contains(x.Id) && string.IsNullOrEmpty(x.EDRPOU));
                var employee = orgEmployee.FirstOrDefault(x => x.OrganizationId == organization.Id);
                if (organization == null)
                {
                    throw new Exception();
                }

                return (organization.Id.ToString(), employee.Id, profile.Id, employee.PersonId);
            }
        }

        public (Guid organizationId, Guid employeeId, Guid profileId, Guid personId) SaveSignIn(SignInEditModel model,
            string path)
        {
            var organization = new OrganizationExt();
            if (!string.IsNullOrEmpty(model.EDRPOU))
            {
                organization = _context.Organizations.FirstOrDefault(x => x.EDRPOU == model.EDRPOU);
            }
            else
            {
                organization = null;
            }

            if (organization == null)
            {
                organization = new OrganizationExt
                {
                    Name = model.Name,
                    EDRPOU = model.EDRPOU,
                    EMail = model.OrgEmail,
                    INN = !string.IsNullOrEmpty(model.EDRPOU) ? null : model.INN
                };

                _context.Add_Auditable(organization);
                _context.SaveChanges();

                var licenses = _limsService.GetLicenses("",
                    !string.IsNullOrEmpty(model.EDRPOU) ? model.EDRPOU : model.INN).Result;

                var trlLicenseIds = new List<string> {"2", "3", "4"};

                var orgInfoPRL = new OrganizationInfo()
                {
                    Name = model.Name,
                    Type = "PRL",
                    OrganizationId = organization.Id,
                    IsActualInfo = true,
                    IsPendingLicenseUpdate = licenses.Any(x => x.LicenseTypesIds.Contains("1"))
                };
                var orgInfoIML = new OrganizationInfo()
                {
                    Name = model.Name,
                    Type = "IML",
                    OrganizationId = organization.Id,
                    IsActualInfo = true,
                    IsPendingLicenseUpdate = licenses.FirstOrDefault(x => x.LicenseTypesIds.Contains("5")) != null
                };
                var orgInfoTRL = new OrganizationInfo()
                {
                    Name = model.Name,
                    Type = "TRL",
                    OrganizationId = organization.Id,
                    IsActualInfo = true,
                    IsPendingLicenseUpdate = licenses.Any(x => x.LicenseTypesIds.Split("|").Any(y => trlLicenseIds.Contains(y)))
                };
                _context.AddRange_Auditable(orgInfoPRL, orgInfoIML, orgInfoTRL);
                _context.SaveChanges();

                //TODO email
            }

            var person = _context.Person.FirstOrDefault(x => x.IPN == model.INN && x.LastName == model.LastName);
            if (person == null)
            {
                person = new Person
                {
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Name = model.UserName,
                    Phone = model.UserPhone,
                    IPN = model.INN,
                    Email = model.UserEmail,
                    Birthday = DateTime.MinValue,
                    Caption = $"{model.LastName} {model.UserName} {model.MiddleName}"
                };
                _context.Add_Auditable(person);
                _context.SaveChanges();
            }

            var employees = _context.EmployeesExt.Where(x => x.PersonId == person.Id)
                .Include(x => x.DefaultValues)
                .Include(x => x.Profiles)
                .ToList();
            var employeeOrg = employees.FirstOrDefault(x => x.OrganizationId == organization.Id);
            var profile = _context.Profiles.FirstOrDefault(x => x.Caption == "Employee");
            if (employeeOrg != null)
            {
                employeeOrg.Position = model.Position;
                employeeOrg.UserEmail = model.UserEmail;
                employeeOrg.ReceiveOnChangeAllMessage = model.ReceiveOnChangeAllMessage;
                employeeOrg.ReceiveOnChangeOwnMessage = model.ReceiveOnChangeOwnMessage;
                employeeOrg.ReceiveOnChangeAllApplication = model.ReceiveOnChangeAllApplication;
                employeeOrg.ReceiveOnChangeOwnApplication = model.ReceiveOnChangeOwnApplication;
                employeeOrg.PersonalCabinetStatus = model.PersonalCabinetStatus;
                employeeOrg.ReceiveOnChangeOrgInfo = model.ReceiveOnChangeOrgInfo;
                employeeOrg.ReceiveOnOverduePayment = model.ReceiveOnOverduePayment;

                try
                {
                    if (employeeOrg.DefaultValues.FirstOrDefault()?.ValueId != organization.Id)
                    {
                        employeeOrg.DefaultValues = new List<UserDefaultValue>
                        {
                            new UserDefaultValue
                            {
                                Caption = person.Caption,
                                EntityName = nameof(OrganizationExt),
                                ValueId = organization.Id
                            }
                        };
                    }
                }
                catch (Exception)
                {
                    if (employeeOrg.DefaultValues.Any())
                    {
                        employeeOrg.DefaultValues = new List<UserDefaultValue>
                        {
                            new UserDefaultValue
                            {
                                Caption = person.Caption,
                                EntityName = nameof(OrganizationExt),
                                ValueId = organization.Id
                            }
                        };
                    }
                }

                if (employeeOrg.Profiles.Any())
                {
                    employeeOrg.Profiles = new List<UserProfile>
                    {
                        new UserProfile
                        {
                            ProfileId = profile.Id,
                            Caption = $"{profile.Caption}: {person.Caption}"
                        }
                    };
                }

                _context.SaveChanges();
                if (employeeOrg.UserEmail != model.UserEmail)
                {
                    //TODO Email
                }
            }
            else
            {
                employeeOrg = new EmployeeExt
                {
                    PersonId = person.Id,
                    Position = model.Position,
                    UserEmail = model.UserEmail,
                    ReceiveOnChangeAllApplication = model.ReceiveOnChangeAllApplication,
                    ReceiveOnChangeAllMessage = model.ReceiveOnChangeAllMessage,
                    ReceiveOnChangeOwnApplication = model.ReceiveOnChangeOwnApplication,
                    ReceiveOnChangeOwnMessage = model.ReceiveOnChangeOwnMessage,
                    PersonalCabinetStatus = model.PersonalCabinetStatus,
                    ReceiveOnChangeOrgInfo = model.ReceiveOnChangeOrgInfo,
                    ReceiveOnOverduePayment = model.ReceiveOnOverduePayment,
                    OrganizationId = organization.Id,
                    DefaultValues = new List<UserDefaultValue>
                    {
                        new UserDefaultValue
                        {
                            Caption = person.Caption,
                            EntityName = nameof(OrganizationExt),
                            ValueId = organization.Id
                        }
                    },
                    Profiles = new List<UserProfile>
                    {
                        new UserProfile
                        {
                            ProfileId = profile.Id,
                            Caption = $"{profile.Caption}: {person.Caption}"
                        }
                    }
                };
                _context.Add_Auditable(employeeOrg);
                _context.SaveChanges();
                //TODO Email
            }

            return (organization.Id, employeeOrg.Id, profile.Id, person.Id);
        }
    }
}
