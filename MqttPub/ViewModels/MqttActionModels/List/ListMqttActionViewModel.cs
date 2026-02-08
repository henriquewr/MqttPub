using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.MqttActions.Abstractions;
using MqttPub.Pages.MqttAction;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.MqttActionModels.List
{
    public partial class ListMqttActionViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<ListMqttActionItemViewModel> MqttActions { get; set; } = null!;

        private readonly IMqttActionService _mqttActionService;

        public ListMqttActionViewModel(IMqttActionService mqttActionService)
        {
            _mqttActionService = mqttActionService;
        }

        public async Task Initialize()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttActions = await _mqttActionService.ListMqttActions<ListMqttActionItemViewModel, MqttConnectionViewModel>();

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