using MqttPub.Application.Services.AppActions.Abstractions;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using MqttPub.Application.Services.AppActions.Implementations.Models;
using MqttPub.Domain.Entities;
using MqttPub.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.AppActions.Implementations
{
    public class AppActionService : IAppActionService
    {
        private readonly IRepository<AppActionEntity> _appActionRepository;

        public AppActionService(IRepository<AppActionEntity> appActionRepository)
        {
            _appActionRepository = appActionRepository;
        }

        public async Task<IEnumerable<T>> ListAppActions<T>() 
            where T : IAppActionItemModel, new()
        {
            var appActionsList = await _appActionRepository.WhereSelectAsNoTrackingAsync(x => true, x => new T
            {
                Id = x.Id,
                Name = x.Name,
            });

            return appActionsList;
        }

        public async Task<IAppActionModel<TAppActionMqttAction>> GetAppAction<TAppActionMqttAction, TMqttAction>(int appActionId)
            where TAppActionMqttAction : IAppActionMqttActionModel<TMqttAction>, new()
            where TMqttAction : IAppActionMqttActionMqttActionModel, new()
        {
            var appAction = await _appActionRepository.SelectFirstAsNoTrackingAsync(x => x.Id == appActionId, x => new AppActionModel<TAppActionMqttAction>
            {
                Name = x.Name,
                MqttActions = x.MqttActions.OrderBy(mqttAction => mqttAction.Order).Select(mqttAction => new TAppActionMqttAction
                {
                    AppActionMqttActionId = mqttAction.Id,
                    Order = mqttAction.Order,
                    MqttAction = new TMqttAction
                    {
                        Id = mqttAction.MqttAction.Id,
                        Name = mqttAction.MqttAction.Name,
                    },
                })
            });

            return appAction;
        }

        public async Task<int> Create(ICreateAppActionModel createAppActionModel)
        {
            var appAction = new AppActionEntity()
            {
                Name = createAppActionModel.Name!,
                MqttActions = createAppActionModel.MqttActions.Select(x => new AppActionMqttAction
                {
                    Id = x.AppActionMqttActionId,
                    Order = x.Order,
                    MqttActionId = x.MqttActionId,
                }).ToList()
            };

            await _appActionRepository.AddAsync(appAction);

            await _appActionRepository.SaveChangesAsync();

            return appAction.Id;
        }

        public async Task Update(IUpdateAppActionModel updateAppActionModel)
        {
            var appAction = (await _appActionRepository.GetByIdAsync(updateAppActionModel.Id, false))!;

            appAction.Name = updateAppActionModel.Name!;
            appAction.MqttActions = updateAppActionModel.MqttActions.Select(x => new AppActionMqttAction
            {
                Id = x.AppActionMqttActionId,
                Order = x.Order,
                MqttActionId = x.MqttActionId,
                AppActionId = appAction.Id,
            }).ToList();

            await _appActionRepository.SaveChangesAsync();
        }
    }
}
