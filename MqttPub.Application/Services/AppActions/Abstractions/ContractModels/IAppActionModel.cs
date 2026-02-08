using System.Collections.Generic;

namespace MqttPub.Application.Services.AppActions.Abstractions.ContractModels
{
    public interface IAppActionModel<TAppActionMqttAction>
    {
        string Name { get; set; }

        IEnumerable<TAppActionMqttAction> MqttActions { get; set; }
    }

    public interface IAppActionMqttActionModel<TMqttAction>
    {
        int AppActionMqttActionId { set; }
        int Order { set; }
        TMqttAction MqttAction { set; }
    }

    public interface IAppActionMqttActionMqttActionModel
    {
        int Id { set; }
        string Name { set; }
    }
}
