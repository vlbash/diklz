using App.Core.Business.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;


namespace App.Core.Business.Middlewares
{
    public class OwnerMiddleware
    {
        private readonly RequestDelegate _next;
        private OwnerService _ownereService;

        public OwnerMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            _ownereService = (OwnerService)context.RequestServices.GetService(typeof(OwnerService));
            String ownerId = context.Request.Headers["Owner"];
            if (ownerId == null)
                ownerId = context.Request.Cookies["owner"];
            if (ownerId != null)
                _ownereService.OwnerId = Convert.ToInt64(ownerId);
            await _next.Invoke(context);
        }
    }
}
