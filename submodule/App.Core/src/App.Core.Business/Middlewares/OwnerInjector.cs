using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Business.Middlewares { 
    public static class OwnerInjector
    {
        public static IApplicationBuilder UseOwnerMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<OwnerMiddleware>();
        }
    }
}
