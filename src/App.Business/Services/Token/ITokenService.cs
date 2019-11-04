using System;
using System.Threading.Tasks;
using App.Business.ViewModels;
using App.Data.Models;

namespace App.Business.Services.Token
{
    public interface ITokenService
    {
        Task<TokenInfo> GetIdGovUaToken(string code);
        Task<IdGovUaUserInfo> GetIdGovUaUserInfo(TokenInfo tokenModel);

        Task<(string organizationId, Guid employeeId, Guid profileId, Guid personId)> CheckOrgEmployeeUnit(
            IdGovUaUserInfo userInfo);
        (Guid organizationId, Guid employeeId, Guid profileId, Guid personId) SaveSignIn(SignInEditModel model,
            string path);
    }
}
