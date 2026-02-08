using MqttPub.Application.Services.MqttActions.Abstractions;
using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using MqttPub.Application.Services.MqttActions.Implementations.Models;
using MqttPub.Domain.Entities;
using MqttPub.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttActions.Implementations
{
    public class MqttActionService : IMqttActionService
    {
        private readonly IRepository<MqttAction> _mqttActionRepository;

        public MqttActionService(IRepository<MqttAction> mqttActionRepository)
        {
            _mqttActionRepository = mqttActionRepository;
        }

        public async Task<int> Create(ICreateMqttActionModel createMqttActionModel)
        {
            var mqttAction = new MqttAction()
            {
                Name = createMqttActionModel.Name,
                MqttConnectionId = createMqttActionModel.MqttConnectionId,
                MqttMessages = createMqttActionModel.MqttMessages.Select(x => new MqttMessage
                {
                    Id = x.Id,
                    Order = x.Order,
                    Message = x.Message!,
                }).ToList()
            };

            await _mqttActionRepository.AddAsync(mqttAction);

            await _mqttActionRepository.SaveChangesAsync();

            return mqttAction.Id;
        }

        public async Task Update(IUpdateMqttActionModel updateMqttActionModel)
        {
            var mqttAction = (await _mqttActionRepository.GetByIdAsync(updateMqttActionModel.Id, false))!;

            mqttAction.Name = updateMqttActionModel.Name;
            mqttAction.MqttConnectionId = updateMqttActionModel.MqttConnectionId;
            mqttAction.MqttMessages = updateMqttActionModel.MqttMessages.Select(x => new MqttMessage
            {
                Id = x.Id,
                Order = x.Order,
                Message = x.Message!,
            }).ToList();

            await _mqttActionRepository.SaveChangesAsync();
        }

        public async Task<IMqttActionModel> GetMqttAction(int mqttActionId)
        {
            var mqttAction = (await _mqttActionRepository.SelectFirstOrDefaultAsNoTrackingAsync(x => x.Id == mqttActionId, x => new MqttActionModel
            {
                Name = x.Name,
                MqttConnectionId = x.MqttConnectionId,
                MqttMessages = x.MqttMessages.OrderBy(m => m.Order).Select(m => new MqttActionMqttMessageModel
                {
                    Id = m.Id,
                    Order = m.Order,
                    Message = m.Message,
                }),
            }))!;

            return mqttAction;
        }

        public async Task<IEnumerable<T>> ListMqttActions<T, TConnection>()
            where T : IMqttActionItemModel<TConnection>, new()
            where TConnection : IMqttActionItemMqttConnectionModel, new()
        {
            var mqttActions = await _mqttActionRepository.WhereSelectAsNoTrackingAsync(x => true, x => new T
            {
                Id = x.Id,
                Name = x.Name,
                MqttConnection = new TConnection()
                {
                    Id = x.MqttConnectionId,
                    BrokerAddress = x.MqttConnection.BrokerAddress,
                    Topic = x.MqttConnection.Topic,
                },
            });

            return mqttActions;
        }
    }
}
