using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttActions.Abstractions
{
    public interface IMqttActionService
    {
        Task<int> Create(ICreateMqttActionModel createMqttActionModel);
        Task Update(IUpdateMqttActionModel updateMqttActionModel);

        Task<IMqttActionModel> GetMqttAction(int mqttActionId);

        Task<IEnumerable<T>> ListMqttActions<T, TConnection>()
            where T : IMqttActionItemModel<TConnection>, new()
            where TConnection : IMqttActionItemMqttConnectionModel, new();
    }
}
