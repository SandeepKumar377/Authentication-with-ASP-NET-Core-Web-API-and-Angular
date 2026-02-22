using AuthEC.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthEC.API.Extensions
{
    public static class EFCoreExtension
    {
        public static IServiceCollection InjectDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
