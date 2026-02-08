using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionSaveViewModel : ObservableObject, ICreateAppActionModel, IUpdateAppActionModel
    {
        public string? OriginalName { get; set; }

        [ObservableProperty]
        public partial int? Id { get; set; }

        [ObservableProperty]
        public partial string? Name { get; set; }

        [ObservableProperty]
        public required partial ObservableCollection<CreateAppActionMqttActionSaveViewModel> MqttActions { get; set; }

        public bool IsValid()
        {
            var isValid = string.IsNullOrWhiteSpace(Name) == false
                && MqttActions.Count != 0 && MqttActions.Select(x => x.Order).SequenceEqual(Enumerable.Range(1, MqttActions.Count));

            return isValid;
        }

        int IUpdateAppActionModel.Id => Id!.Value;
        string ISaveAppActionModel<IUpdateAppActionMqttAction>.Name => Name!;
        string ISaveAppActionModel<ICreateAppActionMqttAction>.Name => Name!;
        IEnumerable<IUpdateAppActionMqttAction> ISaveAppActionModel<IUpdateAppActionMqttAction>.MqttActions => MqttActions;
        IEnumerable<ICreateAppActionMqttAction> ISaveAppActionModel<ICreateAppActionMqttAction>.MqttActions => MqttActions;
    }
}