using System;
using System.Collections.Generic;
using System.Text;

namespace MqttPub.Application.Services.AppActions.Abstractions.ContractModels
{
    public interface ICreateAppActionModel : ISaveAppActionModel<ICreateAppActionMqttAction>
    {
    }

    public interface ICreateAppActionMqttAction : ISaveAppActionMqttAction
    {
    }
}
