using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MqttPub.ViewModels.MqttActionModels.Create
{
    public partial class CreateMqttActionViewModel : ObservableObject
    {
        private readonly IRepository<MqttAction> _mqttActionRepository;
        private readonly IRepository<MqttConnection> _mqttConnectionRepository;

        public CreateMqttActionViewModel(IRepository<MqttAction> mqttActionRepository, IRepository<MqttConnection> mqttConnectionRepository)
        {
            MqttActionSave = new()
            {
                MqttMessages = new()
            };

            MqttActionSave.MqttMessages.CollectionChanged += MqttMessagesChanged;

            _mqttActionRepository = mqttActionRepository;
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

                var mqttConnections = await _mqttConnectionRepository.WhereSelectAsync(x => true, x => new MqttConnectionViewModel
                {
                    Id = x.Id,
                    BrokerAddress = x.BrokerAddress,
                    Topic = x.Topic,
                });

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

                var mqttAction = (await _mqttActionRepository.SelectFirstAsNoTrackingAsync(x => x.Id == id, x => new
                {
                    x.Name,
                    x.MqttConnectionId,
                    MqttMessages = x.MqttMessages.OrderBy(m => m.Order).Select(m => new CreateMqttMessageSaveViewModel
                    {
                        Id = m.Id,
                        Order = m.Order,
                        Message = m.Message,
                    }),
                }))!;

                MqttActionSave.Id = id;
                MqttActionSave.Name = mqttAction.Name;
                
                MqttActionSave.MqttConnection = MqttConnections.First(x => x.Id == mqttAction.MqttConnectionId);

                MqttActionSave.MqttMessages.Clear();
                foreach (var item in mqttAction.MqttMessages)
                {
                    MqttActionSave.MqttMessages.Add(item);
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
        public async Task Save()
        {
            if (!MqttActionSave.IsValid())
            {
                return;
            }

            MqttAction mqttAction;

            if (MqttActionSave.Id.HasValue)
            {
                mqttAction = (await _mqttActionRepository.GetByIdAsync(MqttActionSave.Id.Value, false))!;

                mqttAction.Name = MqttActionSave.Name!;
                mqttAction.MqttConnectionId = MqttActionSave.MqttConnection!.Id;
                mqttAction.MqttMessages = MqttActionSave.MqttMessages.Select(x => new MqttMessage
                {
                    Id = x.Id,
                    Order = x.Order,
                    Message = x.Message!,
                    MqttActionId = MqttActionSave.Id.Value,
                }).ToList();
            }
            else
            {
                mqttAction = new()
                {
                    Name = MqttActionSave.Name!,
                    MqttConnectionId = MqttActionSave.MqttConnection!.Id,
                };
                mqttAction.MqttMessages = MqttActionSave.MqttMessages.Select(x => new MqttMessage
                {
                    Id = x.Id,
                    Order = x.Order,
                    Message = x.Message!,
                    MqttAction = mqttAction,
                }).ToList();
            }

            _mqttActionRepository.Update(mqttAction);

            try
            {
                await _mqttActionRepository.SaveChangesAsync();

                await Shell.Current.CurrentPage.DisplayAlert("Success!", $"The MqttAction {mqttAction.Name} was {(MqttActionSave.Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
        }
    }
}
