using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MqttPub.Application.Services.AppActions.Abstractions;
using MqttPub.Application.Services.AppActions.Implementations;
using MqttPub.Application.Services.MqttActions.Abstractions;
using MqttPub.Application.Services.MqttActions.Implementations;
using MqttPub.Application.Services.MqttConnections.Abstractions;
using MqttPub.Application.Services.MqttConnections.Implementations;
using MqttPub.Application.Services.MqttPub.Abstractions;
using MqttPub.Application.Services.MqttPub.Implementations;
using MqttPub.Domain.DependencyInjection;

namespace MqttPub.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddApplicationServices()
            {
                services.AddSingleton<MqttClientFactory>();
                services.AddTransient<IAppActionService, AppActionService>();
                services.AddTransient<IMqttActionService, MqttActionService>();
                services.AddTransient<IMqttConnectionService, MqttConnectionService>();
                services.AddTransient<IMqttPublisher, MqttPublisher>();

                services.AddDomainServices();

                return services;
            }
        }
    }
}
