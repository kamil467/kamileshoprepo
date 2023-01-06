using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace KamilCataLogAPI.Extensions
{
    public static class SecurityConfiguration
    {
        public static void AddIdentityServerConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                    .AddJwtBearer(option =>
                    {
                        option.Authority = "http://localhost:5001";
                        option.TokenValidationParameters.ValidateAudience = false;
                        option.RequireHttpsMetadata = false;
                    });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("ApiScope", policy =>
                {
                    //policy.RequireAuthenticatedUser();// This will evaulate ID token -- for user delegated access
                    policy.RequireClaim("scope", "catalog-api"); // this will evaulate access token -- machine to machine communication
                });
            });
        }
    }
}
