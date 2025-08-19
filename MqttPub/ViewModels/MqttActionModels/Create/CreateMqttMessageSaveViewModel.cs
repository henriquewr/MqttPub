using CommunityToolkit.Mvvm.ComponentModel;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttMessageSaveViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public required partial string? Message { get; set; }

        [ObservableProperty]
        public required partial int Order { get; set; }
    }
}