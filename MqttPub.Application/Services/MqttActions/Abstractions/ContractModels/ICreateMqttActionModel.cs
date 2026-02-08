namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface ICreateMqttActionModel : ISaveMqttActionModel<ICreateMqttActionMqttMessageModel>
    {
    }

    public interface ICreateMqttActionMqttMessageModel : ISaveMqttMessageModel
    {
    }
}
