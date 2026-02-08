using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.MqttConnections.Abstractions;
using MqttPub.Pages.MqttConnection;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttConnectionModels.List
{
    public partial class ListMqttConnectionViewModel : ObservableObject
    {
        private readonly IMqttConnectionService _mqttConnectionService;

        [ObservableProperty]
        public partial ObservableCollection<MqttConnectionViewModel> MqttConnections { get; set; } = new();

        public ListMqttConnectionViewModel(IMqttConnectionService mqttConnectionService)
        {
            _mqttConnectionService = mqttConnectionService;
        }
        
        public async Task InitializeAsync()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var dataFromDb = await _mqttConnectionService.ListConnections<MqttConnectionViewModel>();

                MqttConnections = new(dataFromDb);
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = false;
                });
            }
        }

        [RelayCommand]
        public async Task EditConnection(MqttConnectionViewModel item)
        {
            await Shell.Current.GoToAsync(CreateMqttConnectionPage.Url(item.Id));
        }

        [RelayCommand]
        public async Task CreateConnection()
        {
            await Shell.Current.GoToAsync(nameof(CreateMqttConnectionPage));
        }
    }
}