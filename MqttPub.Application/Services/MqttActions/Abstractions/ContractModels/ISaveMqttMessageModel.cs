
namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface ISaveMqttMessageModel
    {
        int Id { get; }
        int Order { get; }
        string Message { get; }
    }
}
