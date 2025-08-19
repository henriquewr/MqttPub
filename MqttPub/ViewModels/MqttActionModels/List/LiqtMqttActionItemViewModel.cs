using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.ViewModels.MqttConnectionModels;

namespace MqttPub.ViewModels.MqttActionModels.List
{
    public partial class ListMqttActionItemViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public required partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public required partial string Name { get; set; }

        public string DisplayName => $"{Name}, Connection: {MqttConnection.DisplayName}";

        [ObservableProperty]
        public required partial MqttConnectionViewModel MqttConnection { get; set; }
    }
}