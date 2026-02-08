using System.Collections.Generic;

namespace MqttPub.Application.Services.MqttActions.Abstractions.ContractModels
{
    public interface ISaveMqttActionModel<TMessage>
    {
        string Name { get; }
        int MqttConnectionId { get; }

        IEnumerable<TMessage> MqttMessages { get; }
    }
}
