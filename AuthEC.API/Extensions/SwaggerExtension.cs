using Microsoft.OpenApi.Models;

namespace AuthEC.API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthEC.API", Version = "v1" });
            });
            return services;
        }
        
        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthEC.API v1"));
            }
            return app;
        }
    }
}
