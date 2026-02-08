using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.MqttActions.Abstractions.ContractModels;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttActionSaveViewModel : ObservableObject, ICreateMqttActionModel, IUpdateMqttActionModel
    {
        public CreateMqttActionSaveViewModel()
        {
            
        }

        [ObservableProperty]
        public partial int? Id { get; set; }

        [ObservableProperty]
        public partial string? Name { get; set; }

        [ObservableProperty]
        public partial MqttConnectionViewModel? MqttConnection { get; set; }

        [ObservableProperty]
        public required partial ObservableCollection<CreateMqttMessageSaveViewModel> MqttMessages { get; set; }

        public bool IsValid()
        {
            var isValid = string.IsNullOrWhiteSpace(Name) == false
                && MqttConnection is not null
                && MqttMessages.Count != 0 && MqttMessages.Select(x => x.Order).SequenceEqual(Enumerable.Range(1, MqttMessages.Count));

            return isValid;
        }

        int IUpdateMqttActionModel.Id => Id!.Value;
        int ISaveMqttActionModel<IUpdateMqttActionMqttMessageModel>.MqttConnectionId => MqttConnection!.Id;
        int ISaveMqttActionModel<ICreateMqttActionMqttMessageModel>.MqttConnectionId => MqttConnection!.Id;

        IEnumerable<IUpdateMqttActionMqttMessageModel> ISaveMqttActionModel<IUpdateMqttActionMqttMessageModel>.MqttMessages => MqttMessages;
        IEnumerable<ICreateMqttActionMqttMessageModel> ISaveMqttActionModel<ICreateMqttActionMqttMessageModel>.MqttMessages => MqttMessages;
        string ISaveMqttActionModel<ICreateMqttActionMqttMessageModel>.Name => Name!;
    }
}