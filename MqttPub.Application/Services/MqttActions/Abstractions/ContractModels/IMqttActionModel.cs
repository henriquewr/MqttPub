using System.Collections.Generic;

namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface IMqttActionModel
    {
        string Name { get; }
        int MqttConnectionId { get; }
        IEnumerable<IMqttActionMqttMessageModel> MqttMessages { get; }
    }

    public interface IMqttActionMqttMessageModel
    {
        int Id { get; }
        int Order { get; }
        string Message { get; }
    }
}
