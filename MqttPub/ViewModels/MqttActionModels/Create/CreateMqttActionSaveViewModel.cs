using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttActionSaveViewModel : ObservableObject
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
    }
}