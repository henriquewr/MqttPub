using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttMessageSaveViewModel : ObservableObject, ICreateMqttActionMqttMessageModel, IUpdateMqttActionMqttMessageModel
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public required partial string? Message { get; set; }

        [ObservableProperty]
        public required partial int Order { get; set; }
    }
}