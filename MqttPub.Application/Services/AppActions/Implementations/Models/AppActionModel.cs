using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using System.Collections.Generic;

namespace MqttPub.Application.Services.AppActions.Implementations.Models
{
    internal class AppActionModel<TAppActionMqttAction> : IAppActionModel<TAppActionMqttAction>
    {
        public required string Name { get; set; }
        public required IEnumerable<TAppActionMqttAction> MqttActions { get; set; }
    }
}
