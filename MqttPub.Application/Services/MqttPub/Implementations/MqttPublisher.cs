using MQTTnet;
using MQTTnet.Protocol;
using MqttPub.Application.Services.MqttPub.Abstractions;
using MqttPub.Domain.Entities;
using MqttPub.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttPub.Implementations
{
    public class MqttPublisher : IMqttPublisher
    {
        private readonly MqttClientFactory _mqttFactory;
        private readonly IRepository<AppActionEntity> _appActionRepository;

        public MqttPublisher(MqttClientFactory mqttFactory, IRepository<AppActionEntity> appActionRepository)
        {
            _mqttFactory = mqttFactory;
            _appActionRepository = appActionRepository;
        }

        public async Task RunAction(int appActionId, CancellationToken cancellationToken = default)
        {
            var appActions = await _appActionRepository.SelectFirstAsNoTrackingAsync(x => x.Id == appActionId, x => 
                x.MqttActions.OrderBy(mqttAction => mqttAction.Order)
                .Select(mqttAction => new
                {
                    Messages = mqttAction.MqttAction.MqttMessages.OrderBy(mqttMessage => mqttMessage.Order)
                    .Select(mqttMessage => new
                    {
                        mqttMessage.Order,
                        mqttMessage.Message,
                    }),
                    mqttAction.MqttAction.MqttConnection,
                })
            );

            using var mqttClient = _mqttFactory.CreateMqttClient();

            foreach (var item in appActions)
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(item.MqttConnection.BrokerAddress, item.MqttConnection.Port)
                    .WithClientId(item.MqttConnection.ClientId)
                    .WithTimeout(TimeSpan.FromMinutes(10))
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

                foreach (var message in item.Messages)
                {
                    var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(item.MqttConnection.Topic)
                        .WithPayload(message.Message)
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                        .Build();

                    await mqttClient.PublishAsync(applicationMessage, cancellationToken);
                }

                await mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
