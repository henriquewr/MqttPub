using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using MqttPub.ViewModels.MqttConnectionModels;

namespace MqttPub.ViewModels.MqttActionModels.List
{
    public partial class ListMqttActionItemViewModel : ObservableObject, IMqttActionItemModel<MqttConnectionViewModel>
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial string Name { get; set; }

        public string DisplayName => $"{Name}, Connection: {MqttConnection.DisplayName}";

        [ObservableProperty]
        public partial MqttConnectionViewModel MqttConnection { get; set; }
    }
}