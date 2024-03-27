using Microsoft.EntityFrameworkCore;
using Weather.Precipitation.DataAccess;

namespace Weather.Precipitation.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PrecipDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.UseNpgsql(config.GetConnectionString("AppDb"));
            });

            return services;
        }
    }
}
