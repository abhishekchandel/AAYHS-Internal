using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAYHS.API.Logging
{
    public static class MiddlewareExtension
    {

        public static void ConfigureCustomApplicationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApplicationMiddleware>();
        }
    }
}
