using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.AppActions.Abstractions;
using MqttPub.Application.Services.AppActions.Abstractions.ContractModels;
using MqttPub.Application.Services.AppActions.Implementations.Models;
using MqttPub.Application.Services.MqttActions.Abstractions;
using MqttPub.Domain.Entities;
using MqttPub.ViewModels.MqttActionModels;
using MqttPub.ViewModels.MqttActionModels.List;
using MqttPub.ViewModels.MqttConnectionModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace MqttPub.ViewModels.AppActionModels.Create
{
    public partial class CreateAppActionViewModel : ObservableObject
    {
        private readonly IAppActionService _appActionService;
        private readonly IMqttActionService _mqttActionService;

        [ObservableProperty]
        public partial ObservableCollection<ListMqttActionItemViewModel> MqttActions { get; set; } = null!;

        [ObservableProperty]
        public partial CreateAppActionSaveViewModel AppActionSave { get; set; }

        public CreateAppActionViewModel(IAppActionService appActionService, IMqttActionService mqttActionService)
        {
            AppActionSave = new()
            {
                MqttActions = new()
            };

            AppActionSave.MqttActions.CollectionChanged += MqttActionsChanged;

            _appActionService = appActionService;
            _mqttActionService = mqttActionService;
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

        public async Task LoadAppAction(int id)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var appAction = await _appActionService.GetAppAction<CreateAppActionMqttActionSaveViewModel, MqttActionMinimalViewModel>(id);

                AppActionSave.Id = id;
                AppActionSave.Name = appAction.Name;
                AppActionSave.OriginalName = appAction.Name;

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

            try
            {
                if (AppActionSave.Id.HasValue)
                {
                    if (AppActionSave.OriginalName != AppActionSave.Name)
                    {
                        if (!await Shell.Current.CurrentPage.DisplayAlertAsync("Name changed", "Changing the name may cause other apps remove the action", "Ok", "Cancel"))
                        {
                            return;
                        }
                    }

                    await _appActionService.Update(AppActionSave);
                }
                else
                {
                    await _appActionService.Create(AppActionSave);
                }

                var actions = (await _appActionService.ListAppActions<AppActionItemModel>()).Select(x => new AppAction(x.Id.ToString(), x.Name));

                await AppActions.SetAsync(actions);

                await Shell.Current.CurrentPage.DisplayAlertAsync("Success!", $"The AppAction {AppActionSave.Name} was {(AppActionSave.Id.HasValue ? "updated" : "created")}", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error!", ex.Message, "Ok");
            }
        }
    }
}
