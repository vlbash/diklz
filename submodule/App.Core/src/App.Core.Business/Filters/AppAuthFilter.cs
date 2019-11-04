using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace App.Core.Business.Filters
{
    public class AppAuthFilter: IAuthorizationFilter
    {
        private readonly IUserInfoService _userService;
        private readonly IMemoryCache _memoryCache;
        private static RedirectToActionResult _redirectToActionResult;

        public AppAuthFilter(IMemoryCache memoryCache, IUserInfoService userService, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _userService = userService;
            if (_redirectToActionResult == null)
            {
                _redirectToActionResult = new RedirectToActionResult(config["AuthRedirectSettings:Action"], config["AuthRedirectSettings:Controller"], 
                    new Dictionary<string, string> { { "area", config["AuthRedirectSettings:Area"] } });
            }
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerAction))
            {
                return;
            }

            var conMeth = $"{(context.ActionDescriptor as ControllerActionDescriptor).ControllerName}_" +
                             $"{(context.ActionDescriptor as ControllerActionDescriptor).ActionName}";

            if (!_memoryCache.TryGetValue(conMeth, out bool allowAnnonymous))
            {
                allowAnnonymous = controllerAction.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
                _memoryCache.Set(conMeth, allowAnnonymous);
            }
            
            if (!allowAnnonymous)
            {
                var userInfo = _userService.GetCurrentUserInfo();
                if (string.IsNullOrEmpty(userInfo?.Id))
                {
                    return;
                }
                if (userInfo == null || userInfo.ProfileId == Guid.Empty)
                {
                    context.Result = _redirectToActionResult;
                }
            }
        }
    }
}
