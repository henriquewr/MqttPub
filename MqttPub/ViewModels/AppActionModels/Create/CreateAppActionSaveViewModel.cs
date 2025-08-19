using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionSaveViewModel : ObservableObject
    {
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
    }
}