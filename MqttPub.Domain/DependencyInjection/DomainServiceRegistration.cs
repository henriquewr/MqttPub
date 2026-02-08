using Microsoft.Extensions.DependencyInjection;

namespace MqttPub.Domain.DependencyInjection
{
    public static class DomainServiceRegistration
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddDomainServices()
            {

                return services;
            }
        }
    }
}
