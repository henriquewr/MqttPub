using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.MqttActions.Abstractions;
using MqttPub.Application.Services.MqttConnections.Abstractions;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttActionViewModel : ObservableObject
    {
        private readonly IMqttActionService _mqttActionService;
        private readonly IMqttConnectionService _mqttConnectionService;

        public CreateMqttActionViewModel(IMqttActionService mqttActionService, IMqttConnectionService mqttConnectionService)
        {
            MqttActionSave = new()
            {
                MqttMessages = new()
            };

            MqttActionSave.MqttMessages.CollectionChanged += MqttMessagesChanged;

            _mqttActionService = mqttActionService;
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

                var mqttConnections = await _mqttConnectionService.ListConnections<MqttConnectionViewModel>();

                MqttConnections = new(mqttConnections);
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = false;
                });
            }
        }

        private void MqttMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < MqttActionSave.MqttMessages.Count; i++)
            {
                MqttActionSave.MqttMessages[i].Order = i + 1;
            }
        }

        public async Task LoadMqttAction(int id)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttAction = await _mqttActionService.GetMqttAction(id);

                MqttActionSave.Id = id;
                MqttActionSave.Name = mqttAction.Name;
                
                MqttActionSave.MqttConnection = MqttConnections.First(x => x.Id == mqttAction.MqttConnectionId);

                MqttActionSave.MqttMessages.Clear();
                foreach (var item in mqttAction.MqttMessages)
                {
                    MqttActionSave.MqttMessages.Add(new CreateMqttMessageSaveViewModel
                    {
                        Id = item.Id,
                        Message = item.Message,
                        Order = item.Order,
                    });
                }
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = false;
                });
            }
        }

        [ObservableProperty]
        public partial CreateMqttActionSaveViewModel MqttActionSave { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MqttConnectionViewModel> MqttConnections { get; set; } = null!;

        public bool NewMqttMessageIsValid => !string.IsNullOrWhiteSpace(NewMqttMessage);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NewMqttMessageIsValid))]
        public partial string? NewMqttMessage { get; set; }

        [RelayCommand]
        public void AddNewMqttMessage()
        {
            MqttActionSave.MqttMessages.Add(new CreateMqttMessageSaveViewModel 
            {
                Message = NewMqttMessage,
                Order = MqttActionSave.MqttMessages.Count + 1,
            });

            NewMqttMessage = null;
        }

        [RelayCommand]
        public void RemoveMqttMessage(CreateMqttMessageSaveViewModel mqttMessage)
        {
            MqttActionSave.MqttMessages.Remove(mqttMessage);
        }

        [RelayCommand]
        public async Task Save()
        {
            if (!MqttActionSave.IsValid())
            {
                return;
            }

            try
            {
                await (MqttActionSave.Id.HasValue ? _mqttActionService.Update(MqttActionSave) : _mqttActionService.Create(MqttActionSave));

                await Shell.Current.CurrentPage.DisplayAlertAsync("Success!", $"The MqttAction {MqttActionSave.Name} was {(MqttActionSave.Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error!", ex.Message, "Ok");
            }
        }
    }
}
