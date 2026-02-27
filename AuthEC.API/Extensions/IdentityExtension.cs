using AuthEC.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthEC.API.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }
        
        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 5;
            });
            return services;
        }
        
        #region JWT configuration
        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration configuration)
        {
            int expiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:JWTSecret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });
            return services;
        }
        #endregion

        public static WebApplication AddIdentityMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
