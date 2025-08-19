using CommunityToolkit.Mvvm.ComponentModel;

namespace MqttPub.ViewModels.AppActionModels.List
{
    public partial class ListAppActionItemViewModel : ObservableObject
    {
        [ObservableProperty]
        public required partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public required partial string Name { get; set; }

        public string DisplayName => Name;
    }
}