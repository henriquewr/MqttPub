using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.ViewModels.MqttActionModels;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionMqttActionSaveViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial int AppActionMqttActionId { get; set; }

        [ObservableProperty]
        public required partial int Order { get; set; }

        [ObservableProperty]
        public required partial MqttActionMinimalViewModel MqttAction { get; set; }
    }
}