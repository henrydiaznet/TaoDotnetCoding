using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotnetCoding.Core.Interfaces;
using DotnetCoding.Infrastructure.Repositories;

namespace DotnetCoding.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EshopContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("EshopConnection"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();

            return services;
        }
    }
}
