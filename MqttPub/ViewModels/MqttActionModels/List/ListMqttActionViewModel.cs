using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;
using MqttPub.Pages.MqttAction;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttActionModels.List
{
    public partial class ListMqttActionViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<ListMqttActionItemViewModel> MqttActions { get; set; } = null!;

        private readonly IRepository<MqttAction> _mqttActionRepository;

        public ListMqttActionViewModel(IRepository<MqttAction> mqttActionRepository)
        {
            _mqttActionRepository = mqttActionRepository;
        }

        public async Task Initialize()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttActions = await _mqttActionRepository.WhereSelectAsNoTrackingAsync(x => true, x => new ListMqttActionItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MqttConnection = new MqttConnectionViewModel
                    { 
                        Id = x.MqttConnectionId, 
                        BrokerAddress = x.MqttConnection.BrokerAddress,
                        Topic = x.MqttConnection.Topic,
                    },
                });

                MqttActions = new(mqttActions);
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
        public async Task CreateMqttAction()
        {
            await Shell.Current.GoToAsync(nameof(CreateMqttActionPage));
        }

        [RelayCommand]
        public async Task EditMqttAction(ListMqttActionItemViewModel mqttAction)
        {
            await Shell.Current.GoToAsync(CreateMqttActionPage.Url(mqttAction.Id));
        }
    }
}