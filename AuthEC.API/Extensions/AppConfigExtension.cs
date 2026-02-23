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
    }
}
