using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MqttPub.Data;
using MqttPub.Data.Entities;
using MqttPub.Pages.AppAction;
using System.Collections.ObjectModel;

namespace MqttPub.ViewModels.AppActionModels.List
{
    public partial class ListAppActionViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial ObservableCollection<ListAppActionItemViewModel> AppActions { get; set; } = null!;

        private readonly IRepository<AppActionEntity> _appActionRepository;

        public ListAppActionViewModel(IRepository<AppActionEntity> appActionRepository)
        {
            _appActionRepository = appActionRepository;
        }

        public async Task Initialize()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Shell.Current.CurrentPage.IsBusy = true;
                });

                var appActions = await _appActionRepository.WhereSelectAsNoTrackingAsync(x => true, x => new ListAppActionItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                });

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