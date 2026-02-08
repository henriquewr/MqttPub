using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using MqttPub.ViewModels.MqttActionModels;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionMqttActionSaveViewModel : ObservableObject, ICreateAppActionMqttAction, IUpdateAppActionMqttAction, IAppActionMqttActionModel<MqttActionMinimalViewModel>
    {
        [ObservableProperty]
        public partial int AppActionMqttActionId { get; set; }

        [ObservableProperty]
        public partial int Order { get; set; }

        [ObservableProperty]
        public partial MqttActionMinimalViewModel MqttAction { get; set; }

        int ISaveAppActionMqttAction.MqttActionId => MqttAction.Id;
    }
}