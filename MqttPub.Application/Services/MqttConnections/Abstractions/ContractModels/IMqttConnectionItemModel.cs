namespace MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels
{
    public interface IMqttConnectionItemModel
    {
        int Id { get; set; }
        string BrokerAddress { get; set; }
        string Topic { get; set; }
    }
}
