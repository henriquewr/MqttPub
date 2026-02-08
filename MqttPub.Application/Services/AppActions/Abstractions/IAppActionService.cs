using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.AppActions.Abstractions
{
    public interface IAppActionService
    {
        Task<IEnumerable<T>> ListAppActions<T>()
            where T : IAppActionItemModel, new();

        Task<IAppActionModel<TAppActionMqttAction>> GetAppAction<TAppActionMqttAction, TMqttAction>(int appActionId)
            where TAppActionMqttAction : IAppActionMqttActionModel<TMqttAction>, new()
            where TMqttAction : IAppActionMqttActionMqttActionModel, new();

        Task<int> Create(ICreateAppActionModel createAppActionModel);
        Task Update(IUpdateAppActionModel updateAppActionModel);
    }
}
