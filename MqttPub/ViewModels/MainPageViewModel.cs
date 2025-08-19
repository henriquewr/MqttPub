using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Pages.AppAction;
using MqttPub.Pages.MqttAction;
using MqttPub.Pages.MqttConnection;

namespace MqttPub.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task ListMqttConnection()
        {
            await Shell.Current.GoToAsync(nameof(ListMqttConnectionPage));
        }

        [RelayCommand]
        public async Task ListMqttAction()
        {
            await Shell.Current.GoToAsync(nameof(ListMqttActionPage));
        }

        [RelayCommand]
        public async Task ListAppAction()
        {
            await Shell.Current.GoToAsync(nameof(ListAppActionPage));
        }
    }
}
