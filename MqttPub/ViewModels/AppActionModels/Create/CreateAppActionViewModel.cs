using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;
using MqttPub.ViewModels.MqttActionModels;
using MqttPub.ViewModels.MqttActionModels.List;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionViewModel : ObservableObject
    {
        private readonly IRepository<AppActionEntity> _appActionRepository;
        private readonly IRepository<MqttAction> _mqttActionRepository;

        [ObservableProperty]
        public partial ObservableCollection<ListMqttActionItemViewModel> MqttActions { get; set; } = null!;

        [ObservableProperty]
        public partial CreateAppActionSaveViewModel AppActionSave { get; set; }

        public CreateAppActionViewModel(IRepository<AppActionEntity> appActionRepository, IRepository<MqttAction> mqttActionRepository)
        {
            AppActionSave = new()
            {
                MqttActions = new()
            };

            AppActionSave.MqttActions.CollectionChanged += MqttActionsChanged;

            _appActionRepository = appActionRepository;
            _mqttActionRepository = mqttActionRepository;
        }

        private void MqttActionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < AppActionSave.MqttActions.Count; i++)
            {
                AppActionSave.MqttActions[i].Order = i + 1;
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var mqttActions = await _mqttActionRepository.WhereSelectAsync(x => true, x => new ListMqttActionItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MqttConnection = new()
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

        public async Task LoadAppAction(int id)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var appAction = (await _appActionRepository.SelectFirstAsNoTrackingAsync(x => x.Id == id, x => new
                {
                    x.Name,
                    MqttActions = x.MqttActions.OrderBy(mqttAction => mqttAction.Order).Select(mqttAction => new CreateAppActionMqttActionSaveViewModel
                    {
                        AppActionMqttActionId = mqttAction.Id,
                        Order = mqttAction.Order,
                        MqttAction = new MqttActionMinimalViewModel
                        { 
                            Id = mqttAction.MqttAction.Id,
                            Name = mqttAction.MqttAction.Name,
                        },
                    })
                }))!;

                AppActionSave.Id = id;
                AppActionSave.Name = appAction.Name;

                AppActionSave.MqttActions.Clear();
                foreach (var item in appAction.MqttActions)
                {
                    AppActionSave.MqttActions.Add(item);
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
        [NotifyPropertyChangedFor(nameof(SelectedMqttActionIsValid))]
        public partial ListMqttActionItemViewModel? SelectedMqttAction { get; set; }

        public bool SelectedMqttActionIsValid => SelectedMqttAction is not null;

        [RelayCommand]
        public void AddMqttAction()
        {
            if (SelectedMqttAction is null)
            {
                return;
            }

            AppActionSave.MqttActions.Add(
                new CreateAppActionMqttActionSaveViewModel
                {
                    Order = AppActionSave.MqttActions.Count + 1,
                    MqttAction = new MqttActionMinimalViewModel
                    {
                        Id = SelectedMqttAction.Id,
                        Name = SelectedMqttAction.Name,
                    },
                }
            );

            SelectedMqttAction = null;
        }

        [RelayCommand]
        public async Task Save()
        {
            if (!AppActionSave.IsValid())
            {
                return;
            }

            AppActionEntity appAction;

            if (AppActionSave.Id.HasValue)
            {
                appAction = (await _appActionRepository.GetByIdAsync(AppActionSave.Id.Value, false))!;

                if (appAction.Name != AppActionSave.Name)
                {
                    if (!await Shell.Current.CurrentPage.DisplayAlert("Name changed", "Changing the name may cause other apps remove the action", "Ok", "Cancel"))
                    {
                        return;
                    }
                }

                appAction.Name = AppActionSave.Name!;
                appAction.MqttActions = AppActionSave.MqttActions.Select(x => new AppActionMqttAction
                {
                    Id = x.AppActionMqttActionId,
                    Order = x.Order,
                    MqttActionId = x.MqttAction.Id,
                    AppActionId = appAction.Id,
                }).ToList();
            }
            else
            {
                appAction = new()
                {
                    Name = AppActionSave.Name!,
                };

                appAction.MqttActions = AppActionSave.MqttActions.Select(x => new AppActionMqttAction
                {
                    Id = x.AppActionMqttActionId,
                    Order = x.Order,
                    MqttActionId = x.MqttAction.Id,
                    AppActionId = appAction.Id,
                }).ToList();
            }

            _appActionRepository.Update(appAction);

            try
            {
                await _appActionRepository.SaveChangesAsync();

                var actions = (await _appActionRepository.WhereSelectAsNoTrackingAsync(x => true, x => new
                {
                    Id = x.Id.ToString(),
                    Title = x.Name,
                })).Select(x => new AppAction(x.Id , x.Title));

                await AppActions.SetAsync(actions);

                await Shell.Current.CurrentPage.DisplayAlert("Success!", $"The AppAction {appAction.Name} was {(AppActionSave.Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
        }
    }
}
