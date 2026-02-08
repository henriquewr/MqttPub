using CommunityToolkit.Mvvm.ComponentModel;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;

namespace MqttPub.ViewModels.AppActionModels.List
{
    public partial class ListAppActionItemViewModel : ObservableObject, IAppActionItemModel
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial string Name { get; set; }

        public string DisplayName => Name;
    }
}