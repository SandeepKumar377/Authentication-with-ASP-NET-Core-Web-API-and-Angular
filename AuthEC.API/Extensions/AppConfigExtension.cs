using AuthEC.API.Data;

namespace AuthEC.API.Extensions
{
    public static class AppConfigExtension
    {
        public static WebApplication ConfigureCORS(this WebApplication app,IConfiguration configuration)
        {
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins("http://localhost:4200");
            });
            return app;
        }
        public static IServiceCollection AddAppConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("JWT"));
            return services;
        }
    }
}
