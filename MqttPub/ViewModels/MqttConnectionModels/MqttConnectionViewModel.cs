using CommunityToolkit.Mvvm.ComponentModel;

namespace MqttPub.ViewModels.MqttConnectionModels
{
    public partial class MqttConnectionViewModel : ObservableObject
    {
        [ObservableProperty]
        public required partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public required partial string BrokerAddress { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public required partial string Topic { get; set; }

        
        public string DisplayName => $"{BrokerAddress}, {Topic}";
    }
}