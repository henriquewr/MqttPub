using CachedEfCore.DependencyInjection;
using CachedEfCore.Interceptors;
using CachedEfCore.SqlAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MqttPub.Domain.Repositories;

namespace MqttPub.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddInfrastructureServices(string dbPath)
            {
                services.AddCachedEfCore<GenericSqlQueryEntityExtractor>();

                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

                services.AddDbContext<AppDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlite("Data Source=" + dbPath).AddInterceptors(serviceProvider.GetRequiredService<DbStateInterceptor>());
                });

                return services;
            }
        }
    }
}
