using CommunityToolkit.Mvvm.ComponentModel;

namespace MqttPub.ViewModels.MqttActionModels
{
    public partial class MqttActionMinimalViewModel : ObservableObject
    {
        [ObservableProperty]
        public required partial int Id { get; set; }

        [ObservableProperty]
        public required partial string Name { get; set; }
    }
}
