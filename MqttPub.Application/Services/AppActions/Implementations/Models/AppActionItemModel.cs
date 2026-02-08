using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;

namespace MqttPub.Application.Services.AppActions.Implementations.Models
{
    public class AppActionItemModel : IAppActionItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
