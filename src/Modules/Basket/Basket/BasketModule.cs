using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container

            // Api EndPoint services

            // Application Use Case services
            services.AddScoped<IBasketRepository, BasketRepository>();

            // Data - Infrastructure services
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<BasketDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                    );

                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            return services;
        }

        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            // use Data -Infrastructure services
            app.UseMigration<BasketDbContext>();

            return app;
        }
    }
}
