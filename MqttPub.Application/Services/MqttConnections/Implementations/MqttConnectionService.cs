using MqttPub.Application.Services.MqttConnections.Abstractions;
using MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels;
using MqttPub.Application.Services.MqttConnections.Implementations.Models;
using MqttPub.Domain.Entities;
using MqttPub.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttConnections.Implementations
{
    public class MqttConnectionService : IMqttConnectionService
    {
        private readonly IRepository<MqttConnection> _mqttConnectionRepository;

        public MqttConnectionService(IRepository<MqttConnection> mqttConnectionRepository)
        {
            _mqttConnectionRepository = mqttConnectionRepository;
        }
     
        public async Task<IEnumerable<T>> ListConnections<T>() 
            where T : IMqttConnectionItemModel, new()
        {
            var mqttConnections = await _mqttConnectionRepository.WhereSelectAsync(x => true, x => new T
            {
                Id = x.Id,
                BrokerAddress = x.BrokerAddress,
                Topic = x.Topic,
            });

            return mqttConnections;
        }

        public async Task<IMqttConnectionModel> GetConnection(int mqttConnectionId)
        {
            var mqttConnection = await _mqttConnectionRepository.SelectFirstAsNoTrackingAsync(x => x.Id == mqttConnectionId, x => new MqttConnectionModel
            {
                BrokerAddress = x.BrokerAddress,
                ClientId = x.ClientId,
                Topic = x.Topic,
                Port = x.Port,
            });

            return mqttConnection;
        }

        public async Task<int> Create(ICreateMqttConnectionModel createMqttConnectionModel)
        {
            var mqttConnection = new MqttConnection()
            {
                BrokerAddress = createMqttConnectionModel.BrokerAddress,
                Topic = createMqttConnectionModel.Topic,
                ClientId = createMqttConnectionModel.ClientId,
                Port = createMqttConnectionModel.Port,
            };

            await _mqttConnectionRepository.AddAsync(mqttConnection);

            await _mqttConnectionRepository.SaveChangesAsync();

            return mqttConnection.Id;
        }

        public async Task Update(IUpdateMqttConnectionModel updateMqttConnectionModel)
        {
            var mqttConnection = await _mqttConnectionRepository.GetByIdAsync(updateMqttConnectionModel.Id);

            mqttConnection!.BrokerAddress = updateMqttConnectionModel.BrokerAddress;
            mqttConnection.Topic = updateMqttConnectionModel.Topic;
            mqttConnection.ClientId = updateMqttConnectionModel.ClientId;
            mqttConnection.Port = updateMqttConnectionModel.Port;

            await _mqttConnectionRepository.SaveChangesAsync();
        }
    }
}
