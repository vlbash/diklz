using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Core.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace App.Core.Mvc.Filters
{
    public class ApiAuthFilter: IAuthorizationFilter
    {
        private readonly IUserInfoService _userService;
        private readonly IMemoryCache _memoryCache;

        public ApiAuthFilter(IMemoryCache memoryCache, IUserInfoService userService)
        {
            _memoryCache = memoryCache;
            _userService = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Allow Anonymous skips all authorization
            if (!context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                var userInfo = _userService.GetCurrentUserInfo();
                if (userInfo == null || string.IsNullOrEmpty(userInfo?.Id) || userInfo.ProfileId == Guid.Empty)
                {
                    context.Result = new CustomUnauthorizedResult("Authorization failed.");
                }
            }
        }
    }

    public class CustomUnauthorizedResult: JsonResult
    {
        public CustomUnauthorizedResult(string message)
            : base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
    public class CustomError
    {
        public string Error { get; }

        public CustomError(string message)
        {
            Error = message;
        }
    }

}
