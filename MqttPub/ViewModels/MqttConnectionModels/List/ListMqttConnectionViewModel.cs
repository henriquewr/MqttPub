using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;
using MqttPub.Pages.MqttConnection;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttConnectionModels.List
{
    public partial class ListMqttConnectionViewModel : ObservableObject
    {
        private readonly IRepository<MqttConnection> _mqttConnectionRepository;

        [ObservableProperty]
        public partial ObservableCollection<MqttConnectionViewModel> MqttConnections { get; set; } = new();

        public ListMqttConnectionViewModel(IRepository<MqttConnection> mqttConnectionRepository)
        {
            _mqttConnectionRepository = mqttConnectionRepository;
        }
        
        public async Task InitializeAsync()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });
                var dataFromDb = await _mqttConnectionRepository.WhereSelectAsync(x => true, x => new MqttConnectionViewModel
                {
                    Id = x.Id,
                    BrokerAddress = x.BrokerAddress,
                    Topic = x.Topic,
                });

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