namespace AuthEC.API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            return services;
        }
        
        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            return app;
        }
    }
}
