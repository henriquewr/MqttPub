using MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels;

namespace MqttPub.Application.Services.MqttConnections.Implementations.Models
{
    public class MqttConnectionModel : IMqttConnectionModel
    {
        public required int Port { get; set; }
        public required string BrokerAddress { get; set; }
        public required string Topic { get; set; }
        public required string ClientId { get; set; }
    }
}
