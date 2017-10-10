namespace Microsoft.AspNetCore.Builder
{
    public static class SecurityBuilderExtensions
    {
        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseIdentityServer();

            return app;
        }
    }
}
