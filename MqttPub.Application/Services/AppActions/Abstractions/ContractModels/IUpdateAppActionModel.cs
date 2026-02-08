namespace MqttPub.Application.Services.AppActions.Abstractions.ContractModels
{
    public interface IUpdateAppActionModel : ISaveAppActionModel<IUpdateAppActionMqttAction>
    {
        int Id { get; }
    }

    public interface IUpdateAppActionMqttAction : ISaveAppActionMqttAction
    {
    }
}
