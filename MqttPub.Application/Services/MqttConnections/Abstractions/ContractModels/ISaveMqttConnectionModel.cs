
namespace MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels
{
    public interface ISaveMqttConnectionModel
    {
        string BrokerAddress { get; }
        string Topic { get; }
        string ClientId { get; }
        int Port { get; }
    }
}
