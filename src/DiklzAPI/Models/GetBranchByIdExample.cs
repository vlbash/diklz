using System;
using App.WebApi.Models;
using Swashbuckle.AspNetCore.Examples;

namespace WebApi.Models
{
    public class GetBranchByIdExample: IExamplesProvider
    {
        public object GetExamples() => new SingleResponse<Branch>
        {
            DidError = false,
            ErrorMessage = "",
            Message = "",
            Model = new Branch
            {
                Id = 1,
                LicenseId = 1,
                StatusName = "Status",
                Name = "Name",
                Type = "Type",
                LicTypes = "LicTypes",
                RegionCode = "00000",
                ResidenceTypeName = "ResidenceType",
                PostIndex = "00000",
                Address = "Kuiv",
                RegistrationDate = DateTime.Now.ToShortDateString(),
                TerminateDate = DateTime.Now.ToShortDateString()
            }
        };
    }
}
