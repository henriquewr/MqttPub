using MqttPub.ViewModels.MqttConnectionModels.List;

namespace MqttPub.Pages.MqttConnection;

public partial class ListMqttConnectionPage : ContentPage
{
	private readonly ListMqttConnectionViewModel _listMqttConnectionViewModel;

    public ListMqttConnectionPage(ListMqttConnectionViewModel listMqttConnectionViewModel)
	{
		InitializeComponent();
		BindingContext = listMqttConnectionViewModel;
        _listMqttConnectionViewModel = listMqttConnectionViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		await _listMqttConnectionViewModel.InitializeAsync().ConfigureAwait(false);
    }
}