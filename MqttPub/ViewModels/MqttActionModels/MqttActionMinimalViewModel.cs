using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;

namespace MqttPub.ViewModels.MqttActionModels
{
    public partial class MqttActionMinimalViewModel : ObservableObject, IAppActionMqttActionMqttActionModel
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public partial string Name { get; set; }
    }
}
