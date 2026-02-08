using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using System.Collections.Generic;

namespace MqttPub.Application.Services.MqttActions.Implementations.Models
{
    public class MqttActionModel : IMqttActionModel
    {
        public required string Name { get; set; }
        public required int MqttConnectionId { get; set; }
        public required IEnumerable<IMqttActionMqttMessageModel> MqttMessages { get; set; }
    }

    public class MqttActionMqttMessageModel : IMqttActionMqttMessageModel
    {
        public required int Id { get; set; }
        public required int Order { get; set; }
        public required string Message { get; set; }
    }
}
