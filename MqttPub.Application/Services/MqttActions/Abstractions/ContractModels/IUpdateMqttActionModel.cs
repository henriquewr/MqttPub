namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface IUpdateMqttActionModel : ISaveMqttActionModel<IUpdateMqttActionMqttMessageModel>
    {
        int Id { get; }
    }

    public interface IUpdateMqttActionMqttMessageModel : ISaveMqttMessageModel
    {
    }
}
