namespace MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels
{
    public interface IUpdateMqttConnectionModel : ISaveMqttConnectionModel
    {
        int Id { get; }
    }
}
