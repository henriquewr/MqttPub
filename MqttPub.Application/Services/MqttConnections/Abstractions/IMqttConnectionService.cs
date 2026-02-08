using MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttConnections.Abstractions
{
    public interface IMqttConnectionService
    {
        Task<int> Create(ICreateMqttConnectionModel createMqttConnectionModel);
        Task Update(IUpdateMqttConnectionModel updateMqttConnectionModel);

        Task<IMqttConnectionModel> GetConnection(int mqttConnectionId);
        Task<IEnumerable<T>> ListConnections<T>()
            where T : IMqttConnectionItemModel, new();
    }
}
