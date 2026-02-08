namespace MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels
{
    public interface IMqttConnectionModel
    {
        int Port { get; set; }
        string BrokerAddress { get; set; }
        string Topic { get; set; }
        string ClientId { get; set; }
    }
}
