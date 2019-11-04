using App.Core.Data.DTO.Org;
using Xunit;

namespace App.Core.Tests.CoreDTOModelsTests.ORG
{
    public class OrgDepartmentDTOTests
    {
        private readonly BaseTestCoreDTO _base;

        public OrgDepartmentDTOTests()
        {
            _base = new BaseTestCoreDTO();
        }

        [Fact]
        public void CanReadOrgDepartmentDetailDto()
        {
            _base.CanReadDTO<OrgDepartmentDetailDTO>();
        }
    }
}
