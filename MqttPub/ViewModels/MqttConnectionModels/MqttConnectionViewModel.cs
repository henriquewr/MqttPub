using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using MqttPub.Application.Services.MqttConnections.Abstractions.ContractModels;

namespace MqttPub.ViewModels.MqttConnectionModels
{
    public partial class MqttConnectionViewModel : ObservableObject, IMqttConnectionItemModel, IMqttActionItemMqttConnectionModel
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial string BrokerAddress { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial string Topic { get; set; } = string.Empty;

        public string DisplayName => $"{BrokerAddress}, {Topic}";
    }
}