using System;
using System.Collections.Generic;
using System.Text;

namespace MqttPub.Application.Services.AppActions.Abstractions.ContractModels
{
    public interface IAppActionItemModel
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
