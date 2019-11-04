using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Base;
using App.Core.Data.CustomAudit;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Security;
using App.Core.Security.Entities;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.CRV;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using App.Data.Models.TRL;
using App.Data.Models.NTF;
using App.Data.Models.FDB;
using App.Data.Models.P902;
using Microsoft.EntityFrameworkCore;
using Profile = App.Core.Security.Entities.Profile;

namespace App.Data.Contexts
{
    public static class DbInitializer
    {
        public static void InitializeDictionaries(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }

        public static void SeedSecurityData(ApplicationDbContext context)
        {
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.EnumRecord.Any())
                return;

            Guid id = Guid.NewGuid();

            var orz = new OrganizationExt
            {
                Id = new Guid(),
                RecordState = RecordState.N,
                Caption = "test caption",
                ModifiedBy = Guid.Empty,
                ModifiedOn = DateTime.Now,
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Name = "test name",
                Code = "test code",
                ParentId = null,
                Description = "",
                State = "",
                Category = "",
                EDRPOU = "1234567890",
                EMail = "test email"
            };


            context.Organizations.AddRange(orz);
            context.SaveChanges();

            context.AddRange(
                new PrlApplication
                {
                    Id = id,
                    RecordState = RecordState.N,
                    RegDate = DateTime.Now,
                    AppType = "prl type 1",
                    AppState = "app state 1",
                    AppSort = "app sort 1",
                    OrgUnitId = orz.Id,
                    IsPostDelivery = true,
                    Duns = "some duns",
                    Comment = "test comment",
                    RegNumber = "test reg num"
                },
                new LimsDoc()
                {
                    Id = id,
                    RegDate = DateTime.Now
                }
            );
            context.SaveChanges();

            id = Guid.NewGuid();

            context.AddRange(
                new ImlApplication()
                {
                    RecordState = RecordState.N,
                    IsPostDelivery = true,
                    Duns = "some duns",
                    Comment = "test comment",
                    RegNumber = "test reg num",
                    Id = id,
                    RegDate = DateTime.Now,
                    AppType = "iml type 1",
                    AppState = "app state 2",
                    AppSort = "app sort 2",
                    OrgUnitId = orz.Id
                },
                new LimsDoc()
                {
                    Id = id,
                    RegDate = DateTime.Now
                }
            );
            context.SaveChanges();

            id = Guid.NewGuid();

            context.AddRange(
                new TrlApplication()
                {
                    RecordState = RecordState.N,
                    IsPostDelivery = true,
                    Duns = "some duns",
                    Comment = "test comment",
                    RegNumber = "test reg num",
                    Id = id,
                    RegDate = DateTime.Now,
                    AppType = "trl type 1",
                    AppState = "app state 3",
                    AppSort = "app sort 3",
                    OrgUnitId = orz.Id
                },
                new LimsDoc()
                {
                    Id = id,
                    RegDate = DateTime.Now
                }
            );

            var person = new Person
            {
                Name = "Тестович",
                MiddleName = "Тестов",
                LastName = "Тест",
                IPN = "87654321",
                Birthday = DateTime.MinValue,
                Caption = "Тест Тестович Тестов"
            };

            var adminPerson = new Person
            {
                Name = "Admin",
                MiddleName = "Adminov",
                LastName = "Adminovich",
                Birthday = DateTime.MinValue,
                Caption = "Adminovich Adminov Admin",
                UserId = "8ecf47b2-ba8c-49f9-b901-c496f3bc57a9"
            };

            context.AddRange(person, adminPerson);
            context.SaveChanges();

            context.AddRange(
                new Models.MSG.Message()
                {
                    CreatedBy = person.Id,
                    CreatedOn = DateTime.Now,
                    MessageNumber = "1234567",
                    MessageType = "test message type",
                    MessageState = "Submitted",
                    MessageText = "test message text",
                    OrgUnitId = orz.Id
                }
            );
            context.SaveChanges();

            #region rights

            var kornienko = new EmployeeExt
            {
                PersonId = person.Id
            };

            var adminEmployee = new EmployeeExt
            {
                PersonId = adminPerson.Id,
                UserEmail = "admin@bitsoft.ua"
            };

            context.AddRange(kornienko, adminEmployee);
            context.SaveChanges();

            var profile1 = new Profile
            {
                IsActive = true,
                Caption = "Employee"
            };

            var profileAdmin = new Profile
            {
                IsActive = true,
                Caption = "Admin"
            };

            context.Profiles.AddRange(profile1, profileAdmin);

            var roleForRls = new Role
            {
                IsActive = true,
                Caption = "Employee"
            };

            var roleAdmin = new Role
            {
                IsActive = true,
                Caption = "Admin role"
            };

            context.Roles.AddRange(roleForRls, roleAdmin);



            // rightsAll.AddRange(rightsAll);
            var rightsAll = GetRights();
            context.Rights.AddRange(rightsAll.all);
            context.SaveChanges();

            var apprlr1 = new ApplicationRowLevelRight
            {
                Caption = nameof(OrganizationExt),
                EntityName = nameof(OrganizationExt),
                IsActive = true
            };
            context.ApplicationRowLevelRights.AddRange(apprlr1);

            var rls1 = new RowLevelRight
            {
                ProfileId = profile1.Id,
                EntityName = nameof(OrganizationExt),
                AccessType = RowLevelAccessType.Default
            };
            var adminrls = new RowLevelRight()
            {
                ProfileId = profileAdmin.Id,
                EntityName = nameof(OrganizationExt),
                AccessType = RowLevelAccessType.All
            };
            context.RowLevelRights.AddRange(rls1, adminrls);

            var kornienkoProfileRole1 = new ProfileRole
            {
                ProfileId = profile1.Id,
                RoleId = roleForRls.Id,
                Caption = "Employee"
            };

            var adminProfileRole = new ProfileRole
            {
                ProfileId = profileAdmin.Id,
                RoleId = roleAdmin.Id,
                Caption = "Admin"
            };
            context.ProfileRoles.AddRange(kornienkoProfileRole1, adminProfileRole);
            context.SaveChanges();

            //add all rolerights for role rls
            foreach (var right in rightsAll.all)
            {
                context.Add(new RoleRight
                {
                    RoleId = roleForRls.Id,
                    RightId = right.Id,
                    Caption = "Role: " + roleForRls.Caption + ", right: " + right.Caption
                });
            }

            //add all rolerights for admin
            foreach (var right in rightsAll.all)
            {
                context.Add(new RoleRight
                {
                    RoleId = roleAdmin.Id,
                    RightId = right.Id,
                    Caption = "Role: " + roleAdmin.Caption + ", right: " + right.Caption
                });
            }

            kornienko.Profiles = new List<UserProfile>
            {
                new UserProfile
                {
                    ProfileId = profile1.Id,
                    Caption = profile1.Caption + ": " + person.Caption
                }
            };

            adminEmployee.Profiles = new List<UserProfile>
            {
                new UserProfile
                {
                    ProfileId = profileAdmin.Id,
                    Caption = profileAdmin.Caption + ": " + adminPerson.Caption
                }
            };
            context.SaveChanges();

            #endregion

            context.AddRange_Auditable(new OrganizationExt()
            {
                Name = "test",
                Caption = "testOrg",
                EDRPOU = "1234567896",
                EMail = "pes@ya.ua"
            });
            context.SaveChanges();

            InsertEnums(context);

            InsertCounties(context);
        }

        public static void InsertEnums(ApplicationDbContext context)
        {

            context.AddRange(

            #region Форми власності

                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "10 Приватна власність",
                    Code = "10"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "20 Колективна власність",
                    Code = "20"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "30 Державна власність",
                    Code = "30"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "31 Загальнодержавна власність",
                    Code = "31"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "32 Комунальна власність",
                    Code = "32"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "40 Власність інших держав",
                    Code = "40"
                },
                new EnumRecord()
                {
                    EnumType = "OwnershipForm",
                    Name = "50 Власність міжнародних організацій",
                    Code = "50"
                }
                ,
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "110 Фермерське господарство",
                    Code = "110"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "120 Приватне підприємство",
                    Code = "120"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "130 Колективне підприємство",
                    Code = "130"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "140 Державне підприємство",
                    Code = "140"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "160 Дочірнє підприємство",
                    Code = "160"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "170 Іноземне підприємство",
                    Code = "170"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "100 Індивідуальне підприємство",
                    Code = "100"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "145 Казенне підприємство",
                    Code = "145"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "180 Кооператив",
                    Code = "180"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "181 Виробничий кооператив",
                    Code = "181"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "182 Житлово-будівельний кооператив",
                    Code = "182"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "183 Садове товариство",
                    Code = "183"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "184 Гаражний кооператив",
                    Code = "184"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "185 Кооперативна автостоянка",
                    Code = "185"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "186 Сільськогосподарський виробничий кооператив",
                    Code = "186"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "187 Сільськогосподарський обслуговуючий кооператив",
                    Code = "187"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "190 Орендне підприємство",
                    Code = "190"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "200 Селянське (фермерське) господарство",
                    Code = "200"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "210 Споживче товариство",
                    Code = "210"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "220 Спілка споживчих товарів",
                    Code = "220"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "230 Акціонерне товариство",
                    Code = "230"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "231 Відкрите акціонерне товариство",
                    Code = "231"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "232 Закрите акціонерне товариство",
                    Code = "232"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "240 Товариство з обмеженою відповідальністю",
                    Code = "240"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "250 Товариство з додатковою відповідальністю",
                    Code = "250"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "260 Повне товариство",
                    Code = "260"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "270 Командитне товариство",
                    Code = "270"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "280 Асоціація",
                    Code = "280"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "290 Корпорація",
                    Code = "290"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "300 Консорціум",
                    Code = "300"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "310 Концерн",
                    Code = "310"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "320 Об'єднання",
                    Code = "320"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "330 Дочірнє підприємство",
                    Code = "330"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "340 Підприємець",
                    Code = "340"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "400 Організація",
                    Code = "400"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "410 Заклад",
                    Code = "410"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "420 Установа",
                    Code = "420"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "421 Асоціація та інші добровільні об'єднання органів місцевого самоврядування",
                    Code = "421"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "430 Організація орендарів",
                    Code = "430"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "440 Організація покупців",
                    Code = "440"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "450 Політична партія",
                    Code = "450"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "460 Громадська організація",
                    Code = "460"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "461 Творча спілка",
                    Code = "461"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "470 Релігійна спілка",
                    Code = "470"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "480 Профспілкова організація",
                    Code = "480"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "481 Первинна профспілкова організація",
                    Code = "481"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "482 Республіканські профспілки",
                    Code = "482"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "483 Регіональні профспілки",
                    Code = "483"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "484 Обласні профспілки",
                    Code = "484"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "485 Місцеві (міські, районні, сільські) профспілки",
                    Code = "485"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "486 Об'єднання профспілок",
                    Code = "486"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "487 Всеукраїнські профспілки",
                    Code = "487"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "490 Благодійні фонд",
                    Code = "490"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "491 Об'єднання власників майна житлових будинків",
                    Code = "491"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "492 Членська благодійна організація",
                    Code = "492"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "493 Інші благодійні організації",
                    Code = "493"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "494 Благодійна установа",
                    Code = "494"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "500 Філія",
                    Code = "500"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "510 Представництво",
                    Code = "510"
                },
                new EnumRecord()
                {
                    EnumType = "CodeOrganizationalLegalForm",
                    Name = "520 Підрозділ",
                    Code = "520"
                },

            #endregion

            #region ApplicationState

                new EnumRecord()
                {
                    EnumType = "ApplicationState",
                    Name = "Проект заяви",
                    Code = "Project"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationState",
                    Name = "Заяву подано",
                    Code = "Submitted"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationState",
                    Name = "На розгляді",
                    Code = "InReview"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationState",
                    Name = "Рішення прийнято",
                    Code = "Reviewed"
                },

            #endregion

            #region BackOfficeAppState

                new EnumRecord()
                {
                    EnumType = "BackOfficeAppState",
                    Name = "Проект заяви",
                    Code = "Project"
                },
                new EnumRecord()
                {
                    EnumType = "BackOfficeAppState",
                    Name = "Заяву подано",
                    Code = "Submitted"
                },
                new EnumRecord()
                {
                    EnumType = "BackOfficeAppState",
                    Name = "Зареєстровано",
                    Code = "Registered"
                },
                new EnumRecord()
                {
                    EnumType = "BackOfficeAppState",
                    Name = "В обробці",
                    Code = "InReview"
                },
                new EnumRecord()
                {
                    EnumType = "BackOfficeAppState",
                    Name = "Рішення прийнято",
                    Code = "Reviewed"
                },

            #endregion

            #region Expertise
                new EnumRecord()
                {
                    EnumType = "ExpertiseResult",
                    Name = "Позитивний",
                    Code = "Positive"
                },
                new EnumRecord()
                {
                    EnumType = "ExpertiseResult",
                    Name = "Негативний",
                    Code = "Negative"
                },
                //new EnumRecord()
                //{
                //    EnumType = "ExpertiseResult",
                //    Name = null,
                //    Code = "Empty"
                //},
            #endregion

            #region ApplicationSort

                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про отримання ліцензії на провадження діяльності",
                    Code = "GetLicenseApplication"
                },
                //new EnumRecord()
                //{
                //    EnumType = "ApplicationSort",
                //    Name = "Заява на переоформлення ліцензії",
                //    Code = "RenewLicenseApplication"
                //},
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання МПД",
                    Code = "AddBranchApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання інформації про МПД",
                    Code = "AddBranchInfoApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення МПД",
                    Code = "RemBranchApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення інформації про МПД",
                    Code = "RemBranchInfoApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна уповноважених осіб",
                    Code = "ChangeAutPersonApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна контрактних контрагентів",
                    Code = "ChangeContrApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про звуження провадження виду господарської діяльності - Звуження виробництва лікарських засобів",
                    Code = "DecreasePRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про звуження провадження виду господарської діяльності - Звуження імпорту лікарських засобів",
                    Code = "DecreaseIMLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про звуження провадження виду господарської діяльності - Звуження торгівлі лікарських засобів",
                    Code = "DecreaseTRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про розширення провадження виду господарської діяльності - Розширення до виробництва лікарських засобів",
                    Code = "IncreaseToPRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про розширення провадження виду господарської діяльності - Розширення до імпорту лікарських засобів",
                    Code = "IncreaseToIMLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name =
                        "Заява про розширення провадження виду господарської діяльності - Розширення до торгівлі лікарськими засобами",
                    Code = "IncreaseToTRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про анулювання ліцензії",
                    Code = "CancelLicenseApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Доповнення інформації по наявній ліцензії",
                    Code = "AdditionalInfoToLicense"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про зміну (доповнення) переліку лікарських засобів",
                    Code = "ChangeDrugList"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSort",
                    Name = "Заява про заміну переліку лікарських засобів, що імпортує ліцензіат",
                    Code = "ReplacementDrugList"
                },
            #endregion

            #region ApplicationSortShort

                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "Н",
                    Code = "GetLicenseApplication"
                },
                //new EnumRecord()
                //{
                //    EnumType = "ApplicationSortShort",
                //    Name = "П",
                //    Code = "RenewLicenseApplication"
                //},
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "НСП",
                    Code = "AddBranchApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "РПЛФ",
                    Code = "AddBranchInfoApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ПСП",
                    Code = "RemBranchApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗПЛФ",
                    Code = "RemBranchInfoApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗІ-УО",
                    Code = "ChangeAutPersonApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗІ-КК",
                    Code = "ChangeContrApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗВ-В",
                    Code = "DecreasePRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗВ-І",
                    Code = "DecreaseIMLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "ЗВ-Т",
                    Code = "DecreaseTRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "РВ-В",
                    Code = "IncreaseToPRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "РВ-І",
                    Code = "IncreaseToIMLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "РВ-Т",
                    Code = "IncreaseToTRLApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "А",
                    Code = "CancelLicenseApplication"
                },
                new EnumRecord()
                {
                    EnumType = "ApplicationSortShort",
                    Name = "Доповнення",
                    Code = "AdditionalInfoToLicense"
                },

            #endregion

            #region ActivityType

                new EnumRecord()
                {
                    EnumType = "ActivityType",
                    Caption = "Імпорт",
                    Name = "Імпорт лікарських засобів",
                    Code = "IML"
                },
                new EnumRecord()
                {
                    EnumType = "ActivityType",
                    Caption = "Виробництво",
                    Name = "Виробництво лікарських засобів (промислове)",
                    Code = "PRL"
                },
                new EnumRecord()
                {
                    EnumType = "ActivityType",
                    Caption = "Торгівля",
                    Name = "Оптова, роздрібна торгівля лікарськими засобами",
                    Code = "TRL"
                },

            #endregion

            #region TrlActivityType

            new EnumRecord()
            {
                EnumType = "TrlActivityType",
                Name = "Виробництво (виготовлення) лікарських засобів в умовах аптеки",
                Code = "PrlInPharmacies",
                ExParam1 = "4" //LICTYPE_ID in CDC_LICENSE_TYPE in Old Lims
            },
            new EnumRecord()
            {
                EnumType = "TrlActivityType",
                Name = "Оптова торгівля лікарськими засобами",
                Code = "WholesaleOfMedicines",
                ExParam1 = "2" //LICTYPE_ID in CDC_LICENSE_TYPE in Old Lims
            },
            new EnumRecord()
            {
                EnumType = "TrlActivityType",
                Name = "Роздрібна торгівля лікарськими засобами",
                Code = "RetailOfMedicines",
                ExParam1 = "3" //LICTYPE_ID in CDC_LICENSE_TYPE in Old Lims
            },

            #endregion

            #region BranchType

            new EnumRecord()
            {
                EnumType = "BranchType",
                Name = "Аптека",
                Code = "Pharmacy",
                ExParam1 = "2" //TYPE_ID in CDC_LIC_BRANCH_TYPE in Old Lims
            },
            new EnumRecord()
            {
                EnumType = "BranchType",
                Name = "Аптечний склад (база)",
                Code = "PharmacyStorage",
                ExParam1 = "5" //TYPE_ID in CDC_LIC_BRANCH_TYPE in Old Lims
            },
            new EnumRecord()
            {
                EnumType = "BranchType",
                Name = "Аптечний пункт",
                Code = "PharmacyItem",
                ExParam1 = "3" //TYPE_ID in CDC_LIC_BRANCH_TYPE in Old Lims
            },

            #endregion

            #region асептичні умови

            new EnumRecord()
            {
                EnumType = "AsepticConditions",
                Name = "Крім виробництва в асептичних умовах",
                Code = "ExceptPrInAsepticConditions",
                ExParam1 = "2" //ASEPT_ID in CDC_TRL_ASEPT in Old Lims
            },
            new EnumRecord()
            {
                EnumType = "AsepticConditions",
                Name = "Виробництво в асептичних умовах",
                Code = "PrInAsepticConditions",
                ExParam1 = "3" //ASEPT_ID in CDC_TRL_ASEPT in Old Lims
            },

            #endregion

            #region типи повідомлень

                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна ПІБ керівника Суб'єкту господарювання",
                    Code = "SgdChiefNameChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна найменування Суб'єкту господарювання",
                    Code = "SgdNameChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна місця знаходження юридичної особи / фізичної особи підприємця",
                    Code = "OrgFopLocationChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Призупинення провадження діяльності МПД",
                    Code = "MPDActivitySuspension"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Відновлення провадження діяльності МПД",
                    Code = "MPDActivityRestoration"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name =
                        "Закриття місця провадження діяльності для проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності",
                    Code = "MPDClosingForSomeActivity"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name =
                        "Відновлення роботи місця провадження діяльності після проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності",
                    Code = "MPDRestorationAfterSomeActivity"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Уточнення адреси місця провадження діяльності",
                    Code = "MPDLocationRatification"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Заміна завідуючого аптечного пункту",
                    Code = "PharmacyHeadReplacement"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна площі аптечного закладу",
                    Code = "PharmacyAreaChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Заміна назви аптечного закладу",
                    Code = "PharmacyNameChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна договору оренди",
                    Code = "LeaseAgreementChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Заміна або нова редакція Досьє з виробництва",
                    Code = "ProductionDossierChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Зміна постачальника",
                    Code = "SupplierChange"
                },
                new EnumRecord()
                {
                    EnumType = "MessageType",
                    Name = "Повідомити про іншу подію",
                    Code = "AnotherEvent"
                },

            #endregion

            #region стани повідомлень

                new EnumRecord()
                {
                    EnumType = "MessageState",
                    Name = "Проект",
                    Code = "Project"
                },
                new EnumRecord()
                {
                    EnumType = "MessageState",
                    Name = "Подано",
                    Code = "Submitted"
                },
                new EnumRecord()
                {
                    EnumType = "MessageState",
                    Name = "Зареєстровано",
                    Code = "Registered"
                },
                new EnumRecord()
                {
                    EnumType = "MessageState",
                    Name = "Прийнято до відома",
                    Code = "Accepted"
                },
                new EnumRecord()
                {
                    EnumType = "MessageState",
                    Name = "Відхилено",
                    Code = "Rejected"
                },

            #endregion

            #region Message hierarchy types

                new EnumRecord()
                {
                    EnumType = "MessageHierarchyType",
                    Name = "",
                    Code = "Single"
                },
                new EnumRecord()
                {
                    EnumType = "MessageHierarchyType",
                    Name = "",
                    Code = "Parent"
                },
                new EnumRecord()
                {
                    EnumType = "MessageHierarchyType",
                    Name = "",
                    Code = "Child"
                },

            #endregion

            #region contractor types

                new EnumRecord()
                {
                    EnumType = "ContractorType",
                    Name = "Контрактний виробник",
                    Code = "Manufacturer"
                },
                new EnumRecord()
                {
                    EnumType = "ContractorType",
                    Name = "Контрактна лабораторія",
                    Code = "Laboratory"
                },

            #endregion

            #region assignee types

                new EnumRecord()
                {
                    EnumType = "OrgPositionType",
                    Name = "Уповноважена особа",
                    Code = "Authorized"
                },
                new EnumRecord()
                {
                    EnumType = "OrgPositionType",
                    Name = "Завідувач",
                    Code = "Manager"
                },

            #endregion

            #region edoc

                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Досьє з виробництва",
                    Code = "ManufactureDossier"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Досьє з імпорту",
                    Code = "ImportDossier"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Відомості МТБ та персоналу (виробництво в умовах аптеки)",
                    Code = "PayrollPharmacyProduction"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Відомості МТБ та персоналу (роздрібна торгівля)",
                    Code = "PayrollRetail"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Відомості МТБ та персоналу (оптова торгівля)",
                    Code = "PayrollWholesale"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Сертифікат якості, виданий виробником на серію лікарського засобу",
                    Code = "CertificateQualityMedicalProduct"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Документи, що підтверджують відповідність умов виробництва" +
                           " лікарських засобів вимогам до виробництва лікарських засобів в Україні",
                    Code = "DocumentsProductConditions"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Реєстраційне посвідчення на ввезені лікарські засоби",
                    Code = "MedicinesRegistrationCertificate"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Митна документація",
                    Code = "CustomsDocumentation"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Документ, що підтверджує дотримання умов зберігання під " +
                           "час транспортування від виробника до імпортера",
                    Code = "StorageConditionsDocument"
                },
                new EnumRecord()
                {
                    EnumType = "EDocumentType",
                    Name = "Платіж, що підтверджує оплату",
                    Code = "PaymentDocument"
                },

            #endregion

            #region license states

                new EnumRecord
                {
                    EnumType = "LicenseState",
                    Code = "Active",
                    Name = "Діюча"
                },
                new EnumRecord
                {
                    EnumType = "LicenseState",
                    Code = "Canceled",
                    Name = "Анульована"
                },
                new EnumRecord
                {
                    EnumType = "LicenseState",
                    Code = "Invalidated",
                    Name = "Визнана недійсною"
                },
                new EnumRecord
                {
                    EnumType = "LicenseState",
                    Code = "Expired",
                    Name = "Термін закінчився"
                },

            #endregion

            #region ATU

                new EnumRecord { EnumType = "LocalityType", Name = "Село", Code = "Village" },
                new EnumRecord { EnumType = "LocalityType", Name = "Селище", Code = "Hamlet" },
                new EnumRecord
                {
                    EnumType = "LocalityType",
                    Name = "Селища міського типу",
                    Code = "UrbanTypeVillages"
                },
                new EnumRecord
                {
                    EnumType = "LocalityType",
                    Name = "Міста районного підпорядкування",
                    Code = "TownsOfDistrictSubordination"
                },
                new EnumRecord
                {
                    EnumType = "LocalityType",
                    Name = "Міста обласного підпорядкування",
                    Code = "CitiesOfRegionalSubordination"
                },
                new EnumRecord { EnumType = "StreetType", Name = "Вулиця", Code = "Street" },
                new EnumRecord { EnumType = "StreetType", Name = "Провулок", Code = "Lane" },
                new EnumRecord { EnumType = "StreetType", Name = "Бульвар", Code = "Boulevard" },
                new EnumRecord { EnumType = "StreetType", Name = "Проспект", Code = "Avenue" },
                new EnumRecord { EnumType = "StreetType", Name = "Площа", Code = "Square" },
                new EnumRecord { EnumType = "StreetType", Name = "Інше", Code = "Other" },

            #endregion

            #region BranchActivity

                new EnumRecord
                {
                    EnumType = "BranchActivity",
                    Code = "Active",
                    Name = "Активний"
                },
                new EnumRecord
                {
                    EnumType = "BranchActivity",
                    Code = "Suspended",
                    Name = "Призупинено"
                },
                new EnumRecord
                {
                    EnumType = "BranchActivity",
                    Code = "Closed",
                    Name = "Закрито"
                },

            #endregion

            #region DecisionType

                new EnumRecord
                {
                    EnumType = "DecisionType",
                    Code = "Accepted",
                    Name = "Позитивне рішення"
                },
                new EnumRecord
                {
                    EnumType = "DecisionType",
                    Code = "Denied",
                    Name = "Відмова"
                },
                new EnumRecord
                {
                    EnumType = "DecisionType",
                    Code = "WithoutConsideration",
                    Name = "Без розгляду"
                },

            #endregion

            #region Decision reason

                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "Incomplete",
                    Name = "Некомплект"
                },
                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "Disordered",
                    Name = "Оформлено з порушенням"
                },
                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "WithBreachOfTime",
                    Name = "З порушенням строків"
                },
                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "SGDMissingEDG",
                    Name = "СГД відсутній у ЄДГ"
                },
                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "DiscrepancyLU",
                    Name = "Невідповідність ЛУ"
                },
                new EnumRecord
                {
                    EnumType = "DecisionReason",
                    Code = "Inaccuracy",
                    Name = "Недостовірність"
                },

            #endregion

            #region ProtocolState

            new EnumRecord
            {
                EnumType = "ProtocolState",
                Code = "В роботі",
                Name = "В роботі"
            },
            new EnumRecord
            {
                EnumType = "ProtocolState",
                Code = "Закритий",
                Name = "Закритий"
            },

            #endregion

            #region NotificationSort

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppSend_Org",
                Name = "Заяву відправлено на розгляд до ДЛС (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppSend_User",
                Name = "Заяву відправлено на розгляд до ДЛС (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationMsgSend_Org",
                Name = "Повідомлення відправлено на розгляд до ДЛС (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationMsgSend_User",
                Name = "Повідомлення відправлено на розгляд до ДЛС (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppRegister_Org",
                Name = "Заяву зареєстровано в ДЛС (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppRegister_User",
                Name = "Заяву зареєстровано в ДЛС (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppResolve_Org",
                Name = "ДЛС прийняло рішення по заяві, яку було подано (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppResolve_User",
                Name = "ДЛС прийняло рішення по заяві, яку було подано (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppResolvePay_Org",
                Name = "ДЛС прийняло рішення по заяві, яку було подано. Очікується сплата (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppResolvePay_User",
                Name = "ДЛС прийняло рішення по заяві, яку було подано. Очікується сплата (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationAppResolvePayRepeatedly_Org",
                Name = "Нагадування про необхідність внесення оплати по заяві (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationMsgResolve_Org",
                Name = "Прийнято рішення по повідомленню (для організації)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationMsgResolve_User",
                Name = "Прийнято рішення по повідомленню (для користувача)"
            },

            new EnumRecord
            {
                EnumType = "NotificationSort",
                Code = "NotificationDeleteDrafts_User",
                Name = "Автоматичне видалення проектів (для користувача)"
            },

            #endregion

            #region Payment status

            new EnumRecord
            {
                EnumType = "PaymentStatus",
                Code = "DontNeed",
                Name = "Не потребує"
            },

            new EnumRecord
            {
                EnumType = "PaymentStatus",
                Code = "RequiresPayment",
                Name = "Потребує оплати"
            },

            new EnumRecord
            {
                EnumType = "PaymentStatus",
                Code = "PaymentConfirmed",
                Name = "Оплата підтверджена"
            },

            new EnumRecord
            {
                EnumType = "PaymentStatus",
                Code = "WaitingForConfirmation",
                Name = "Очікує підтвердження"
            },

            new EnumRecord
            {
                EnumType = "PaymentStatus",
                Code = "PaymentNotVerified",
                Name = "Оплата не підтверджена"
            },

            #endregion

            #region AuditTableName

            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "PrlApplication",
                Name = "Заява на виробництво"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "Message",
                Name = "Повідомлення"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "PrlContractor",
                Name = "Контрактні контрагенти"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "AppDecision",
                Name = "Рішення"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "PrlLicense",
                Name = "Ліцензія виробництва"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "AppPreLicenseCheck",
                Name = "Предліцензійна перевірка"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "AppAssignee",
                Name = "Уповноважена особа"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "AppLicenseMessage",
                Name = "Повідомлення про результат розгляду"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "FileStore",
                Name = "Сховище файлів"
            },
            new EnumRecord
            {
                EnumType = "AuditTableName",
                Code = "Person",
                Name = "Користувачі"
            },

            #endregion

            #region Код економічної класифікації
            new EnumRecord
            {
                EnumType = "EconomicClassificationCode",
                Name = "24.42.0 Виробництво фармацевтичних препаратів",
                Code = "24.42.0"
            },
            new EnumRecord
            {
                EnumType = "EconomicClassificationCode",
                Name = "51.46.0 Оптова торгівля фармацевтичними товарами",
                Code = "51.46.0"
            },
            new EnumRecord
            {
                EnumType = "EconomicClassificationCode",
                Name = "52.31.0 Роздрібна торгівля фармацевтичними товарами",
                Code = "52.31.0"
            },
            #endregion

            #region P902 TDS
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "Україна",
                Code = "1"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Сумській області",
                Code = "2"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС в АР Крим",
                Code = "3"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Вінницькій області",
                Code = "4"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Волинській області",
                Code = "5"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Дніпропетровській області",
                Code = "6"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Донецькій області",
                Code = "7"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Житомирській області",
                Code = "8"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДC у Закарпатській області",
                Code = "9"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Запорізькій області",
                Code = "10"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС в Івано-Франківській області",
                Code = "11"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Київській області",
                Code = "12"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у м. Києві",
                Code = "13"
            },
            //new EnumRecord
            //{
            //    EnumType = "TeritorialService",
            //    Name = "Державна служба з лікарських засобів та контролю за наркотиками у Кіровоградській області",
            //    Code = "27"
            //},
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Луганській області",
                Code = "15"
            },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Львівській області",
                Code = "16"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Миколаївській області",
                 Code = "17"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС в Одеській області",
                Code = "18"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Полтавській області",
                 Code = "19"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Рівненській області",
                Code = "20"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Тернопільській області",
                 Code = "21"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у м. Севастополі",
                Code = "22"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Харківській області",
                 Code = "23"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Херсонській області",
                Code = "24"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Хмельницькій області",
                 Code = "25"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "ДС у Черкаській області",
                Code = "26"
            },
             new EnumRecord
             {
                 EnumType = "TeritorialService",
                 Name = "ДС у Чернівецькій області",
                 Code = "27"
             },
            new EnumRecord
            {
                EnumType = "TeritorialService",
                Name = "Держлікслужба у Чернігівській області",
                Code = "28"
            },
            #endregion

            #region ResultInputControlState

            new EnumRecord()
            {
                EnumType = "ResultInputControlState",
                Name = "Проект",
                Code = "Project",
                ExParam1 = "1"
            },
            new EnumRecord()
            {
                EnumType = "ResultInputControlState",
                Name = "Відправлено",
                Code = "Sent",
                ExParam1 = "2"
            },
            new EnumRecord()
            {
                EnumType = "ResultInputControlState",
                Name = "Підтверджено",
                Code = "Confirmed",
                ExParam1 = "3"
            },
            new EnumRecord()
            {
                EnumType = "ResultInputControlState",
                Name = "Відхилено",
                Code = "Rejected",
                ExParam1 = "4"
            },

            #endregion

            #region UnitOfMeasurement

            new EnumRecord()
            {
                EnumType = "UnitOfMeasurement",
                Name = "Кілограм",
                Code = "Kilogram",
                ExParam1 = "2"
            },
            new EnumRecord()
            {
                EnumType = "UnitOfMeasurement",
                Name = "Літрів",
                Code = "Liters",
                ExParam1 = "3"
            },
            new EnumRecord()
            {
                EnumType = "UnitOfMeasurement",
                Name = "Упаковки",
                Code = "Packaging",
                ExParam1 = "1"
            },

            #endregion

            #region StatusConclusion
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Обробка",
                    Code = "1"
                },
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Частково видано",
                    Code = "2"
                },
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Справу завершено",
                    Code = "3"
                },
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Знято з контролю",
                    Code = "4"
                },
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Проект",
                    Code = "5"
                },
                new EnumRecord
                {
                    EnumType = "StatusConclusion",
                    Name = "Відправлено проект",
                    Code = "6"
                },
            #endregion

            #region InputControlResult

            new EnumRecord()
            {
                EnumType = "InputControlResult",
                Name = "Відповідає",
                Code = "Eligible",
                ExParam1 = "2"
            },
            new EnumRecord()
            {
                EnumType = "InputControlResult",
                Name = "Не відповідає",
                Code = "NonEligible",
                ExParam1 = "3"
            }

            #endregion
            );

            context.SaveChanges();
        }

        public static void SeedAdmin(ApplicationDbContext context)
        {

            var orz2 = new OrganizationExt
            {
                Id = Guid.NewGuid(),
                RecordState = RecordState.N,
                Caption = "test caption",
                ModifiedBy = Guid.Empty,
                ModifiedOn = DateTime.Now,
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Name = "Держлікслужба",
                Code = "test code",
                ParentId = null,
                Description = "",
                State = "",
                Category = "",
                EDRPOU = "40517815",
                EMail = "test email"
            };
            context.Add(orz2);
            context.SaveChanges();
            string AdminId = "d5fa47a8-0080-4f6d-b084-a544ade5f575";

            context.Database.ExecuteSqlCommand("insert into person (id, record_state, modified_by,created_by,created_on,\"name\",middle_name,last_name,birthday,no_ipn)" + $@"
            select guid, 2, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', current_date, first_name_n, middle_name_n, last_name_n, '0001-01-01 00:00:00', false
            from dict_person;

            insert into org_employee (id, record_state, modified_by, created_by, created_on, person_id, organization_id, discriminator, old_lims_id)
            select guid, 2, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', current_date, guid, '{orz2.Id.ToString()}', 'EmployeeExt', person_id
            from dict_person;");
            var adminPerson1 = new Person
            {
                Name = "Admin",
                MiddleName = "Adminov",
                LastName = "Adminovich",
                Birthday = DateTime.MinValue,
                Caption = "Adminovich Adminov Admin",
                UserId = AdminId
            };

            context.AddRange(adminPerson1);
            context.SaveChanges();

            var adminEmployee1 = new EmployeeExt
            {
                PersonId = adminPerson1.Id,
                OrganizationId = orz2.Id
            };

            context.AddRange(adminEmployee1);
            context.SaveChanges();

            var profileAdmin1 = new Profile
            {
                IsActive = true,
                Caption = "Admin"
            };

            context.Profiles.AddRange(profileAdmin1);

            var roleAdmin1 = new Role
            {
                IsActive = true,
                Caption = "Admin role"
            };

            context.Roles.AddRange(roleAdmin1);

            var adminrls1 = new RowLevelRight()
            {
                ProfileId = profileAdmin1.Id,
                EntityName = nameof(OrganizationExt),
                AccessType = RowLevelAccessType.All
            };
            context.RowLevelRights.AddRange(adminrls1);
            context.SaveChanges();


            var adminProfileRole1 = new ProfileRole
            {
                ProfileId = profileAdmin1.Id,
                RoleId = roleAdmin1.Id,
                Caption = "Admin"
            };

            context.ProfileRoles.AddRange(adminProfileRole1);
            context.SaveChanges();

            var rightsAll1 = context.Rights.ToList();

            foreach (var right in rightsAll1)
            {
                context.Add(new RoleRight
                {
                    RoleId = roleAdmin1.Id,
                    RightId = right.Id,
                    Caption = "Role: " + roleAdmin1.Caption + ", right: " + right.Caption
                });
            }

            context.SaveChanges();

            adminEmployee1.Profiles = new List<UserProfile>
            {
                new UserProfile
                {
                    ProfileId = profileAdmin1.Id,
                    Caption = profileAdmin1.Caption + ": " + adminPerson1.Caption
                }
            };
            context.SaveChanges();
        }

        private static (List<Right> all, List<Right> baseList, List<Right> prl, List<Right> trl, List<Right> iml, List<Right> msg) GetRights()
        {
            var rightOrganizationExt = CreateRight(typeof(OrganizationExt));
            var rightAppAssignee = CreateRight(typeof(AppAssignee));
            var rightAppAssigneeBranch = CreateRight(typeof(AppAssigneeBranch));
            var rightPrlContractor = CreateRight(typeof(PrlContractor));
            var rightApplicationBranch = CreateRight(typeof(ApplicationBranch));
            var rightBaseApplication = CreateRight(typeof(BaseApplication));
            var rightImlApplication = CreateRight(typeof(ImlApplication));
            var reghtImlLicense = CreateRight(typeof(ImlLicense));
            var rightBranch = CreateRight(typeof(Branch));
            var rightEmployeeExt = CreateRight(typeof(EmployeeExt));
            var rightPrlApplication = CreateRight(typeof(PrlApplication));
            var rightPrlLicense = CreateRight(typeof(PrlLicense));
            var rightTrlApplication = CreateRight(typeof(TrlApplication));
            var rightTrlLicense = CreateRight(typeof(TrlLicense));
            var rightLimsDoc = CreateRight(typeof(LimsDoc));
            var rightMessage = CreateRight(typeof(Models.MSG.Message));
            var rightPrlBranchContractor = CreateRight(typeof(PrlBranchContractor));
            var rightEDocument = CreateRight(typeof(EDocument));
            var rightFileStore = CreateRight(typeof(FileStore));
            var rightOrganizationInfo = CreateRight(typeof(OrganizationInfo));
            var rightBranchEDocument = CreateRight(typeof(BranchEDocument));
            #region ATU
            var rightRegion = CreateRight(typeof(Region));
            var rightCity = CreateRight(typeof(City));
            var rightCityDistricts = CreateRight(typeof(CityDistricts));
            var rightStreet = CreateRight(typeof(Street));
            var rightSubjectAddress = CreateRight(typeof(SubjectAddress));
            var rightCountry = CreateRight(typeof(Country));
            #endregion
            var rightAppDecision = CreateRight(typeof(AppDecision));
            var rightAppDecisionReason = CreateRight(typeof(AppDecisionReason));
            var rightAppLicenseMessage = CreateRight(typeof(AppLicenseMessage));
            var rightAppPreLicenseCheck = CreateRight(typeof(AppPreLicenseCheck));
            var rightAppProtocol = CreateRight(typeof(AppProtocol));
            var rightNotification = CreateRight(typeof(Notification));
            var rightFeedback = CreateRight(typeof(Feedback));
            var rightPerson = CreateRight(typeof(Person));
            var rightLimsRP = CreateRight(typeof(LimsRP));
            var rightImlMedicine = CreateRight(typeof(ImlMedicine));
            var rightEntityEnumRecords = CreateRight(typeof(EntityEnumRecords));
            var rightRight = CreateRight(typeof(Right));
            var rightRole = CreateRight(typeof(Role));
            var rightRoleRight = CreateRight(typeof(RoleRight));
            var rightProfile = CreateRight(typeof(Profile));
            var rightProfileRight = CreateRight(typeof(ProfileRight));
            var rightProfileRole = CreateRight(typeof(ProfileRole));
            var rightEmployee = CreateRight(typeof(Employee));
            var rightResultInputControl = CreateRight(typeof(ResultInputControl));
            var rightPharmacyItemPharmacy = CreateRight(typeof(PharmacyItemPharmacy));
            var rightAppConclusion = CreateRight(typeof(AppConclusion));
            var rightConclusionMedicine = CreateRight(typeof(ConclusionMedicine));

            var listBase = new List<Right>
            {
                rightOrganizationExt,
                rightAppAssignee,
                rightAppAssigneeBranch,
                rightApplicationBranch,
                rightBaseApplication,
                rightBranch,
                rightEmployeeExt,
                rightOrganizationExt,
                rightLimsDoc,
                rightPrlBranchContractor,
                rightAppAssignee,
                rightAppAssigneeBranch,
                rightEDocument,
                rightFileStore,
                rightOrganizationInfo,
                rightBranchEDocument,
                rightRegion,
                rightCity,
                rightCityDistricts,
                rightStreet,
                rightSubjectAddress,
                rightCountry,
                rightAppDecision,
                rightAppDecisionReason,
                rightAppLicenseMessage,
                rightAppPreLicenseCheck,
                rightAppProtocol,
                rightNotification,
                rightFeedback,
                rightPerson,
                rightLimsRP,
                rightEntityEnumRecords,
                rightPharmacyItemPharmacy,
                rightAppConclusion,
                rightConclusionMedicine
            };

            var listPrl = new List<Right>
            {
                rightPrlContractor,
                rightPrlBranchContractor,
                rightPrlApplication,
                rightPrlLicense
            };

            var listIml = new List<Right>
            {
                rightImlApplication,
                reghtImlLicense,
                rightImlMedicine
            };

            var listTrl = new List<Right>
            {
                rightTrlApplication,
                rightTrlLicense
            };

            var listMsg = new List<Right>
            {
                rightMessage
            };

            var allList = new List<Right>
            {
                rightOrganizationExt,
                rightAppAssignee,
                rightAppAssigneeBranch,
                rightPrlContractor,
                rightPrlBranchContractor,
                rightApplicationBranch,
                rightBaseApplication,
                rightImlApplication,
                reghtImlLicense,
                rightBranch,
                rightEmployeeExt,
                rightOrganizationExt,
                rightPrlApplication,
                rightPrlLicense,
                rightTrlApplication,
                rightTrlLicense,
                rightLimsDoc,
                rightMessage,
                rightPrlBranchContractor,
                rightAppAssignee,
                rightAppAssigneeBranch,
                rightEDocument,
                rightFileStore,
                rightOrganizationInfo,
                rightBranchEDocument,
                rightRegion,
                rightCity,
                rightCityDistricts,
                rightStreet,
                rightSubjectAddress,
                rightCountry,
                rightAppDecision,
                rightAppDecisionReason,
                rightAppLicenseMessage,
                rightAppPreLicenseCheck,
                rightAppProtocol,
                rightNotification,
                rightFeedback,
                rightPerson,
                rightLimsRP,
                rightImlMedicine,
                rightEntityEnumRecords,
                rightRight,
                rightRole,
                rightRoleRight,
                rightProfile,
                rightProfileRight,
                rightProfileRole,
                rightEmployee,
                rightResultInputControl,
                rightPharmacyItemPharmacy,
                rightAppConclusion,
                rightConclusionMedicine

            };
            return (allList, listBase, prl: listPrl, trl: listTrl, iml: listIml, msg: listMsg);
        }


        private static Right CreateRight(Type type)
        {
            return new Right
            {
                EntityName = type.Name,
                EntityAccessLevel = EntityAccessLevel.Write,
                IsActive = true,
                Caption = type.Name
            };
        }

        public static void RefreshRights(ApplicationDbContext context)
        {
            context.RoleRights.RemoveRange(context.RoleRights);
            context.SaveChanges();
            context.Rights.RemoveRange(context.Rights);
            context.SaveChanges();
            var rights = GetRights();
            context.Rights.AddRange(rights.all);
            context.Rights.AddRange(rights.prl);
            context.Rights.AddRange(rights.trl);
            context.Rights.AddRange(rights.iml);
            context.Rights.AddRange(rights.baseList);
            context.Rights.AddRange(rights.msg);
            context.SaveChanges();
            var roles = context.Roles.ToList();
            var admin = roles.FirstOrDefault(x => x.Caption == "Admin role");
            var employee = roles.FirstOrDefault(x => x.Caption == "Employee");
            var allRoles = new List<Role>() { admin , employee };
            foreach (var role in allRoles)
            {
                foreach (var right in rights.all)
                {
                    context.Add(new RoleRight
                    {
                        RoleId = role.Id,
                        RightId = right.Id,
                        Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                    });
                }
            }

            var prl = roles.FirstOrDefault(x => x.Caption == "PRL");
            foreach (var right in rights.prl)
            {
                context.Add(new RoleRight
                {
                    RoleId = prl.Id,
                    RightId = right.Id,
                    Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                });
            }

            var iml = roles.FirstOrDefault(x => x.Caption == "IML");
            foreach (var right in rights.iml)
            {
                context.Add(new RoleRight
                {
                    RoleId = iml.Id,
                    RightId = right.Id,
                    Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                });
            }
            
            var trl = roles.FirstOrDefault(x => x.Caption == "TRL");
            foreach (var right in rights.trl)
            {
                context.Add(new RoleRight
                {
                    RoleId = trl.Id,
                    RightId = right.Id,
                    Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                });
            }
            
            var baseRole = roles.FirstOrDefault(x => x.Caption == "BASE");
            foreach (var right in rights.trl)
            {
                context.Add(new RoleRight
                {
                    RoleId = baseRole.Id,
                    RightId = right.Id,
                    Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                });
            }
            var msg = roles.FirstOrDefault(x => x.Caption == "MSG");
            foreach (var right in rights.trl)
            {
                context.Add(new RoleRight
                {
                    RoleId = msg.Id,
                    RightId = right.Id,
                    Caption = "Role: " + admin.Caption + ", right: " + right.Caption
                });
            }



            context.SaveChanges();
        }

        public static void AddRoles(ApplicationDbContext context)
        {
            var rights = GetRights();
            AddRoleRights("BASE", rights.baseList, context);
            AddRoleRights("PRL", rights.prl, context);
            AddRoleRights("IML", rights.iml, context);
            AddRoleRights("TRL", rights.trl, context);
            AddRoleRights("MSG", rights.msg, context);
        }

        private static void AddRoleRights(string type, List<Right> rights, ApplicationDbContext context)
        {
            var baseRole = context.Roles.FirstOrDefault(x => x.Caption == type);
            if (baseRole == null)
            {
                var role = new Role()
                {
                    Caption = type,
                    Id = Guid.NewGuid(),
                    IsActive = true
                };
                context.Add(role);
                var rightsBase = context.Rights.ToList();
                foreach (var right in rights)
                {
                    var item = rightsBase.FirstOrDefault(x => x.EntityName == right.EntityName);
                    if (item == null)
                    {
                        right.Id = Guid.NewGuid();
                        context.Rights.Add(right);
                        context.Add(new RoleRight
                        {
                            RoleId = role.Id,
                            RightId = right.Id,
                            Caption = "Role: " + role.Caption + ", right: " + right.Caption
                        });
                    }
                    else
                    {
                        context.Add(new RoleRight
                        {
                            RoleId = role.Id,
                            RightId = item.Id,
                            Caption = "Role: " + role.Caption + ", right: " + right.Caption
                        });
                    }
                }
            }

            context.SaveChanges();
        }

        public static void InsertCounties(ApplicationDbContext context)
        {
            // Справочник стран - http://sokrashhenija.ru/stran/azii.html

            #region Country

            context.AddRange(
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "UKR",
                    Name = "Україна",
                    Caption = "Ukraine"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "AUS",
                    Name = "Австралія",
                    Caption = "Australia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "AUT",
                    Name = "Австрія",
                    Caption = "Austria"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "AZB",
                    Name = "Азербайджан",
                    Caption = "Azerbaijan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "AL",
                    Name = "Албанія",
                    Caption = "Albania"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ALG",
                    Name = "Алжир",
                    Caption = "Algeria"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ATG",
                    Name = "Антігуа і Барбуда",
                    Caption = "Antigua and Barbuda"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ANT",
                    Name = "Антіли Нідерландські",
                    Caption = "Netherlands Antilles"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MAC",
                    Name = "Аоминь (Макао)",
                    Caption = "Macau"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ARG",
                    Name = "Аргентина",
                    Caption = "Argentina"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ABW",
                    Name = "Аруба (Нідерланди)",
                    Caption = "Aruba"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "AFG",
                    Name = "Афганістан",
                    Caption = "Afghanistan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BHS",
                    Name = "Багамські острови",
                    Caption = "The Bahamas"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BGD",
                    Name = "Бангладеш",
                    Caption = "Bangladesh"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BRB",
                    Name = "Барбадос",
                    Caption = "Barbados"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BHR",
                    Name = "Бахрейн",
                    Caption = "Bahrain"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BLZ",
                    Name = "Беліз",
                    Caption = "Belize"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BEL",
                    Name = "Бельгія",
                    Caption = "Belgium"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BEN",
                    Name = "Бенін",
                    Caption = "Benin"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BMU",
                    Name = "Бермудські острови",
                    Caption = "Bermuda"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BGR",
                    Name = "Болгарія",
                    Caption = "Bulgaria"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BOL",
                    Name = "Болівія",
                    Caption = "Bolivia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BIH",
                    Name = "Боснія і Герцеговина",
                    Caption = "Bosnia and Herzegovina"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BWA",
                    Name = "Ботсвана",
                    Caption = "Botswana"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BRA",
                    Name = "Бразилія",
                    Caption = "Brazil"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IOT",
                    Name = "Британська територія Індійського океану",
                    Caption = "British Indian Ocean Territory"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BRN",
                    Name = "Бруней Даруссалам",
                    Caption = "Brunei"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BFA",
                    Name = "Буркіна-Фасо",
                    Caption = "Burkina Faso"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BDI",
                    Name = "Бурунді",
                    Caption = "Burundi"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BTN",
                    Name = "Бутан",
                    Caption = "Bhutan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VNM",
                    Name = "В`єтнам",
                    Caption = "Vietnam"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VUT",
                    Name = "Вануату",
                    Caption = "Vanuatu"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VAT",
                    Name = "Ватікан",
                    Caption = "Vatican City"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GBR",
                    Name = "Великобританія",
                    Caption = "United Kingdom"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GBR1",
                    Name = "Великобританія (ВМП)",
                    Caption = "UK"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VEN",
                    Name = "Венесуела",
                    Caption = "Venezuela"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VIR",
                    Name = "Віргінcькі острови (CША)",
                    Caption = "United States Virgin Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VGB",
                    Name = "Віргінські острови (Британія)",
                    Caption = "British Virgin Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ARM",
                    Name = "Вірменія",
                    Caption = "Armenia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GAB",
                    Name = "Габон",
                    Caption = "Gabon"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GAS",
                    Name = "Газа Сектор (Палестина)",
                    Caption = "Gaza Strip (Palestine)"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HTI",
                    Name = "Гаіті",
                    Caption = "Haiti"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GUY",
                    Name = "Гайана",
                    Caption = "Guyana"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GMB",
                    Name = "Гамбія",
                    Caption = "Gambia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GHA",
                    Name = "Гана",
                    Caption = "Ghana"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GLP",
                    Name = "Гваделупа (Франція)",
                    Caption = "Guadeloupe"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GTM",
                    Name = "Ґватемала",
                    Caption = "Guatemala"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GUF",
                    Name = "Гвіана (Франція)",
                    Caption = "French Guiana"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GIN",
                    Name = "Гвінея",
                    Caption = "Guinea"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GNB",
                    Name = "Гвінея-Бісау",
                    Caption = "Guinea-Bissau"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GIB",
                    Name = "Гібралтар (Британія)",
                    Caption = "Gibraltar"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HOL",
                    Name = "Голландія",
                    Caption = "Holland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HND",
                    Name = "Гондурас",
                    Caption = "Honduras"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HKG",
                    Name = "Гонконг",
                    Caption = "Hong Kong"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GRD",
                    Name = "Гренада",
                    Caption = "Grenada"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GRL",
                    Name = "Гренландія",
                    Caption = "Greenland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GRC",
                    Name = "Греція",
                    Caption = "Greece"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GEO",
                    Name = "Грузія",
                    Caption = "Georgia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GUM",
                    Name = "Гуам (США)",
                    Caption = "Guam"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "DNK",
                    Name = "Данія",
                    Caption = "Denmark"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "DJI",
                    Name = "Джібуті",
                    Caption = "Djibouti"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "JTN",
                    Name = "Джонстон острова",
                    Caption = "Johnston Atoll"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "DMA",
                    Name = "Домініка",
                    Caption = "Dominica"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "DOM",
                    Name = "Домініканська республіка",
                    Caption = "Dominican Republic"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ECU",
                    Name = "Еквадор",
                    Caption = "Ecuador"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "GNQ",
                    Name = "Екваторіальна Гвінея",
                    Caption = "Equatorial Guinea"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SLV",
                    Name = "Ель-Сальвадор",
                    Caption = "El Salvador"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ERI",
                    Name = "Ерітрея",
                    Caption = "Eritrea"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "EST",
                    Name = "Естонія",
                    Caption = "Estonia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ETH",
                    Name = "Ефіопія",
                    Caption = "Ethiopia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "EGY",
                    Name = "Єгипет",
                    Caption = "Egypt"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "YEM",
                    Name = "Ємен",
                    Caption = "Yemen"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ZAR",
                    Name = "Заїр",
                    Caption = "Zaire"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ZMB",
                    Name = "Замбія",
                    Caption = "Zambia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ESH",
                    Name = "Західна Сахара",
                    Caption = "Western Sahara"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "WSM",
                    Name = "Західне Самоа",
                    Caption = "Samoa"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ZWE",
                    Name = "Зімбабве",
                    Caption = "Zimbabwe"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ISR",
                    Name = "Ізраїль",
                    Caption = "Israel"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IND",
                    Name = "Індія",
                    Caption = "India"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IDN",
                    Name = "Індонезія",
                    Caption = "Indonesia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IRQ",
                    Name = "Ірак",
                    Caption = "Iraq"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IRN",
                    Name = "Іран",
                    Caption = "Iran"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IRL",
                    Name = "Ірландія",
                    Caption = "Ireland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ISL",
                    Name = "Ісландія",
                    Caption = "Iceland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ESP",
                    Name = "Іспанія",
                    Caption = "Spain"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ITA",
                    Name = "Італія",
                    Caption = "Italy"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "JOR",
                    Name = "Йорданія",
                    Caption = "Jordan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CPV",
                    Name = "Кабо-Верде",
                    Caption = "Cape Verde"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KAZ",
                    Name = "Казахстан",
                    Caption = "Kazakhstan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CYM",
                    Name = "Острова Кайман (Британія)",
                    Caption = "Cayman Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KHM",
                    Name = "Камбоджа",
                    Caption = "Cambodia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CMR",
                    Name = "Камерун",
                    Caption = "Cameroon"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CAN",
                    Name = "Канада",
                    Caption = "Canada"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "QAT",
                    Name = "Катар",
                    Caption = "Qatar"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KEN",
                    Name = "Кенія",
                    Caption = "Kenya"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KGZ",
                    Name = "Киргизстан",
                    Caption = "Kyrgyzstan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CHN",
                    Name = "Китай",
                    Caption = "China"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CHN1",
                    Name = "Китай",
                    Caption = "P.R.China"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CYP",
                    Name = "Кіпр",
                    Caption = "Cyprus"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KIR",
                    Name = "Кірібаті",
                    Caption = "Kiribati"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PRK",
                    Name = "КНДР",
                    Caption = "DPRK"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CCK",
                    Name = "Кокосові острови",
                    Caption = "Cocos Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "COL",
                    Name = "Колумбія",
                    Caption = "Colombia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "COM",
                    Name = "Коморські острови",
                    Caption = "Comoro Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "COG",
                    Name = "Конго",
                    Caption = "Congo"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KOR",
                    Name = "Корея",
                    Caption = "Korea"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CRI",
                    Name = "Коста-Ріка",
                    Caption = "Costa Rica"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CIV",
                    Name = "Кот-Д'івуар",
                    Caption = "Ivory Coast"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KWT",
                    Name = "Кувейт",
                    Caption = "Kuwait"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LAO",
                    Name = "Лаос",
                    Caption = "Laos"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LVA",
                    Name = "Латвія",
                    Caption = "Latvia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LSO",
                    Name = "Лєсото",
                    Caption = "Lesotho"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LTU",
                    Name = "Литва",
                    Caption = "Lithuania"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LBR",
                    Name = "Ліберія",
                    Caption = "Liberia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LBN",
                    Name = "Ліван",
                    Caption = "Lebanon"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LBY",
                    Name = "Лівія",
                    Caption = "Libya"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LIE",
                    Name = "Ліхтенштейн",
                    Caption = "Liechtenstein"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LUX",
                    Name = "Люксембург",
                    Caption = "Luxembourg"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MMR",
                    Name = "М`янма",
                    Caption = "Myanmar"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MUS",
                    Name = "Маврікій",
                    Caption = "Mauritius"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MRT",
                    Name = "Маврітанія",
                    Caption = "Mauritania"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MDG",
                    Name = "Мадагаскар",
                    Caption = "Madagascar"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MKD",
                    Name = "Македонія",
                    Caption = "Macedonia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MWI",
                    Name = "Малаві",
                    Caption = "Malawi"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MYS",
                    Name = "Малайзія",
                    Caption = "Malaysia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MLI",
                    Name = "Малі",
                    Caption = "Mali"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "UMI",
                    Name = "Малі Тихоокеанські острови",
                    Caption = "United States Minor Outlying Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MDV",
                    Name = "Мальдіви",
                    Caption = "Maldives"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MLT",
                    Name = "Мальта",
                    Caption = "Malta"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MNP",
                    Name = "Марианські острови",
                    Caption = "Mariana Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MAR",
                    Name = "Марокко",
                    Caption = "Morocco"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MTQ",
                    Name = "Мартініка (Франція)",
                    Caption = "Martinique"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MHL",
                    Name = "Маршаллови острови",
                    Caption = "Marshall Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MEX",
                    Name = "Мексика",
                    Caption = "Mexico"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MID",
                    Name = "Мідуейські острови",
                    Caption = "Midway Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FSM",
                    Name = "Мікронезія",
                    Caption = "Mikronesiya"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MOZ",
                    Name = "Мозамбік",
                    Caption = "Mozambique"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MDA",
                    Name = "Молдова",
                    Caption = "Republic Moldova"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MDA1",
                    Name = "Молдова (ВМП)",
                    Caption = "Молдова"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MCO",
                    Name = "Монако",
                    Caption = "Monaco"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MNG",
                    Name = "Монголія",
                    Caption = "Mongolia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MSR",
                    Name = "Монтсеррат (Британія)",
                    Caption = "Montserrat"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NAM",
                    Name = "Намібія",
                    Caption = "Namibia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NRU",
                    Name = "Науру",
                    Caption = "Nauru"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NPL",
                    Name = "Непал",
                    Caption = "Nepal"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NER",
                    Name = "Нігер",
                    Caption = "Niger"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NGA",
                    Name = "Нігерія",
                    Caption = "Nigeria"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NLD",
                    Name = "Нідерланди",
                    Caption = "Netherlands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NLD1",
                    Name = "Нідерланди (ВМП)",
                    Caption = "The Netherlands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NIC",
                    Name = "Нікарагуа",
                    Caption = "Nicaragua"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "DEU",
                    Name = "Німеччина",
                    Caption = "Germany"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NIU",
                    Name = "Ніуе (Нова Зеландія)",
                    Caption = "Republic of Niue"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NZL",
                    Name = "Нова Зеландія",
                    Caption = "New Zeland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NCL",
                    Name = "Нова Каледонія",
                    Caption = "New Caledonia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NOR",
                    Name = "Норвегія",
                    Caption = "Norway"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CNI",
                    Name = "Нормандські острови",
                    Caption = "Channel Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "NFK",
                    Name = "Норфолк",
                    Caption = "Norfolk"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ARE",
                    Name = "Об'єднані Арабські Емірати",
                    Caption = "United Arab Emirates"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "OMN",
                    Name = "Оман",
                    Caption = "Oman"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SJM",
                    Name = "Шпіцберген і Ян-Маєн",
                    Caption = "Svalbard and Jan Mayen"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BVT",
                    Name = "Острів Бувет",
                    Caption = "Bouvet Island"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "IMY",
                    Name = "Острів Мен",
                    Caption = "Isle of Man"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CXR",
                    Name = "Острів Різдва",
                    Caption = "Christmas Island"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SHN",
                    Name = "Острів Святої Єлени",
                    Caption = "St. Helen Island"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "WAK",
                    Name = "Острів Уейк",
                    Caption = "Wake Island"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "COK",
                    Name = "Острови Кука",
                    Caption = "Cook Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PAK",
                    Name = "Пакистан",
                    Caption = "Pakistan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PLW",
                    Name = "Палау острови (США)",
                    Caption = "Palau Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PLS",
                    Name = "Палестина",
                    Caption = "Palestine"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PAN",
                    Name = "Панама",
                    Caption = "Panama"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "XZ",
                    Name = "Паонта Сахиб",
                    Caption = "Paonto Sahub"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PNG",
                    Name = "Папуа-Нова Гвінея",
                    Caption = "Papua New Guinea"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PRY",
                    Name = "Парагвай",
                    Caption = "Paraguay"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PER",
                    Name = "Перу",
                    Caption = "Peru"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ZAF",
                    Name = "Південно-Африк.Респ.",
                    Caption = "South Africa"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PCN",
                    Name = "Піткерн (Британія)",
                    Caption = "Pitcairn"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "POL",
                    Name = "Польща",
                    Caption = "Poland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PRT",
                    Name = "Португалія",
                    Caption = "Portugal"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PRI",
                    Name = "Пуерто-Ріко",
                    Caption = "Puerto Rico"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BLR",
                    Name = "Республіка Білорусь",
                    Caption = "Belorus"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BLR1",
                    Name = "Республіка Білорусь (ВМП)",
                    Caption = "Belorus"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "BLR2",
                    Name = "Республіка Білорусь (ВМП)",
                    Caption = "Belarus"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CUB",
                    Name = "Республіка Куба",
                    Caption = "Cuba"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "REU",
                    Name = "Реюньйон (Франція)",
                    Caption = "Reunion"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "RUS",
                    Name = "Російська Федерація",
                    Caption = "Russian Federation"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "RWA",
                    Name = "Руанда",
                    Caption = "Rwanda"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ROM",
                    Name = "Румунія",
                    Caption = "Romania"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SMR",
                    Name = "Сан-Маріно",
                    Caption = "San Marino"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "STP",
                    Name = "Сан-Томе і Прінсіпі",
                    Caption = "Sao Tome and Principe"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SAU",
                    Name = "Саудівська Аравія",
                    Caption = "Saudi Arabia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SWZ",
                    Name = "Свазіленд",
                    Caption = "Swaziland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SYC",
                    Name = "Сейшельські острови",
                    Caption = "Seychelles"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SEN",
                    Name = "Сенегал",
                    Caption = "Senegal"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SPM",
                    Name = "Сен-П`єр і Мікелон",
                    Caption = "Saint-Pierre and Miquelon"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "VCT",
                    Name = "Сент-Вінсент і Гренадіни",
                    Caption = "Saint Vincent and the Grenadines"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "KNA",
                    Name = "Сент-Кітс і Невіс",
                    Caption = "Saint Cites and Nevis"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LCA",
                    Name = "Сент-Люсія",
                    Caption = "Saint Lucia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SIG",
                    Name = "Сербія",
                    Caption = "Serbia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SYR",
                    Name = "Сирія",
                    Caption = "Syria"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SGP",
                    Name = "Сінгапур",
                    Caption = "Singapore"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SVK",
                    Name = "Словацька Республіка",
                    Caption = "Slovak Republic"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SVN",
                    Name = "Словенія",
                    Caption = "Slovenia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SLB",
                    Name = "Соломонові острови",
                    Caption = "Solomon Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SOM",
                    Name = "Сомалі",
                    Caption = "Somalia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SDN",
                    Name = "Судан",
                    Caption = "Sudan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SUR",
                    Name = "Сурінам",
                    Caption = "Surinam"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TMP",
                    Name = "Східний Тімор",
                    Caption = "Scheme Tіmor"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "USA",
                    Name = "США",
                    Caption = "USA"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SLE",
                    Name = "Сьєрра-Леоне",
                    Caption = "Sierra Leone"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TJK",
                    Name = "Таджикистан",
                    Caption = "Tajikistan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "THA",
                    Name = "Таїланд",
                    Caption = "Thailand"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TWN",
                    Name = "Тайвань (Китай)",
                    Caption = "Taiwan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TZA",
                    Name = "Танзанія",
                    Caption = "Tanzania"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TCA",
                    Name = "Теркс і Кайкос острови",
                    Caption = "Turks and Caicos Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TGO",
                    Name = "Того",
                    Caption = "Togolese Republic"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TKL",
                    Name = "Токелау",
                    Caption = "Tokelau"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TON",
                    Name = "Тонга",
                    Caption = "Tonga"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TTO",
                    Name = "Трінідад і Тобаго",
                    Caption = "Trіnіdad i Tobago"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TUV",
                    Name = "Тувалу",
                    Caption = "Tuvalu"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TUN",
                    Name = "Туніс",
                    Caption = "Тunisie"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TUR",
                    Name = "Туреччина",
                    Caption = "Turkey"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TKM",
                    Name = "Туркменистан",
                    Caption = "Turkmenistan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "UGA",
                    Name = "Уганда",
                    Caption = "Uganda"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HUN",
                    Name = "Угорщина",
                    Caption = "Hungary"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "UZB",
                    Name = "Узбекистан",
                    Caption = "Uzbekistan"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "WLF",
                    Name = "Уоллис і Футуна",
                    Caption = "Wallis and Futuna"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "URY",
                    Name = "Уругвай",
                    Caption = "Uruguay"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FRO",
                    Name = "Фарерські острови",
                    Caption = "Faroe Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FJI",
                    Name = "Фіджі",
                    Caption = "Fiji"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PHL",
                    Name = "Філіппіни",
                    Caption = "Philippines"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FIN",
                    Name = "Фінляндія",
                    Caption = "Finland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FLK",
                    Name = "Фолклендські острови",
                    Caption = "Falkland Islands"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "ATF",
                    Name = "Французькі Південні території",
                    Caption = "French Southern Territories"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "FRA",
                    Name = "Франція",
                    Caption = "France"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "PYF",
                    Name = "Французька Полінезія",
                    Caption = "French Polynesia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HMD",
                    Name = "Херд і Макдональд",
                    Caption = "Heard and McDonald"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "HRV",
                    Name = "Хорватія",
                    Caption = "Croatia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CAF",
                    Name = "Центрально-Африканська Республіка",
                    Caption = "Central African Republic"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "TCD",
                    Name = "Чад",
                    Caption = "Chad"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CSK",
                    Name = "Чеська Республіка",
                    Caption = "Czech Republic"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CHL",
                    Name = "Чилі",
                    Caption = "Chile"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "MNE",
                    Name = "Чорногорія",
                    Caption = "Montenegro"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "CHE",
                    Name = "Швейцарія",
                    Caption = "Switzerland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SWE",
                    Name = "Швеція",
                    Caption = "Sweden"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "SCT",
                    Name = "Шотландія",
                    Caption = "Scotland"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "LKA",
                    Name = "Шрі-Ланка",
                    Caption = "Sri Lanka"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "YUG",
                    Name = "Югославія",
                    Caption = "Jugoslavia"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "JAM",
                    Name = "Ямайка",
                    Caption = "Jamaica"
                },
                new Country()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    RecordState = RecordState.N,
                    Code = "JPN",
                    Name = "Японія",
                    Caption = "Japan"
                }
            );

            #endregion

            context.SaveChanges();
        }
    }
}
