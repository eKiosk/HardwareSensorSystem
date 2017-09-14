using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class SecurityBuilderExtensions
    {
        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            return app;
        }
    }
}
