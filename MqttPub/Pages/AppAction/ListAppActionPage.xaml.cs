using MqttPub.ViewModels.AppActionModels.List;

namespace MqttPub.Pages.AppAction;

public partial class ListAppActionPage : ContentPage
{
	private readonly ListAppActionViewModel _listAppActionViewModel;

    public ListAppActionPage(ListAppActionViewModel listAppActionViewModel)
	{
		InitializeComponent();
        BindingContext = listAppActionViewModel;
        _listAppActionViewModel = listAppActionViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _listAppActionViewModel.Initialize().ConfigureAwait(false);
    }
}