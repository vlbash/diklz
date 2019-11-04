using System;
using System.Collections.Generic;
using App.WebApi.Models;
using Swashbuckle.AspNetCore.Examples;

namespace WebApi.Models
{
    public class GetLicenseByIdExample: IExamplesProvider
    {
        public object GetExamples() => new SingleResponse<License>()
        {
            DidError = false,
            ErrorMessage = "",
            Message = "",
            Model = new License
            {
                Id = 1,
                LicenseStatusId = 1,
                RegNumber = "1",
                EDRPOU = "11111",
                OrganizationName = "OrgName",
                OrganizationAddress = "Kyiv",
                RegistrationDate = DateTime.Now.ToShortDateString(),
                LicenseTypesName = "LicenseType",
                OrganizationFormName = "OrgForm",
                ListOfBranchesString = new List<Branch> { new Branch { Id = 1 }, new Branch { Id = 2 } }.ToString(),
                Branches = new List<Branch>
                {
                    new Branch { Id = 1 },
                    new Branch { Id = 2 }
                }
            }
        };
    }
}
