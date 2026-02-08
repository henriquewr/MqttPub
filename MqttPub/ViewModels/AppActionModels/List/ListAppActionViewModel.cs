using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Application.Services.AppActions.Abstractions;
using MqttPub.Pages.AppAction;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.AppActionModels.List
{
    public partial class ListAppActionViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<ListAppActionItemViewModel> AppActions { get; set; } = null!;

        private readonly IAppActionService _appActionService;

        public ListAppActionViewModel(IAppActionService appActionService)
        {
            _appActionService = appActionService;
        }

        public async Task Initialize()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var appActions = await _appActionService.ListAppActions<ListAppActionItemViewModel>();

                AppActions = new(appActions);
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
        public async Task CreateAppAction()
        {
            await Shell.Current.GoToAsync(nameof(CreateAppActionPage));
        }

        [RelayCommand]
        public async Task EditAppAction(ListAppActionItemViewModel appAction)
        {
            await Shell.Current.GoToAsync(CreateAppActionPage.Url(appAction.Id));
        }
    }
}