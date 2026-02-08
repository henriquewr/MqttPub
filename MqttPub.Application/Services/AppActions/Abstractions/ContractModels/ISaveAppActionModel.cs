using System.Collections.Generic;

namespace MqttPub.Application.Services.AppActions.Abstractions.ContractModels
{
    public interface ISaveAppActionModel<TMqttAction>
    {
        string Name { get; }
        IEnumerable<TMqttAction> MqttActions { get; }
    }

    public interface ISaveAppActionMqttAction
    {
        int AppActionMqttActionId { get; }
        int MqttActionId { get; }
        int Order { get; }
    }
}
