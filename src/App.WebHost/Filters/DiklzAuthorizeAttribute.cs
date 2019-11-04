using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Host.Filters
{
    public class DiklzAuthorizeAttribute : AuthorizationHandler<DiklzAuthorizeAttribute>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DiklzAuthorizeAttribute requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "register"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var redirectContext = context.Resource as AuthorizationFilterContext;

            var isRegistered = context.User.FindFirst(x => x.Type == "register").Value;

            if (isRegistered == "1")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else
            {
                redirectContext.Result = new RedirectToActionResult("SignIn", "Auth", null);
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
    }
}
