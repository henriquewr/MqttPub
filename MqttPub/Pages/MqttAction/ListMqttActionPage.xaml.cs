using MqttPub.ViewModels.MqttActionModels.List;

namespace MqttPub.Pages.MqttAction;

public partial class ListMqttActionPage : ContentPage
{
	private readonly ListMqttActionViewModel _listMqttActionViewModel;

    public ListMqttActionPage(ListMqttActionViewModel listMqttActionViewModel)
	{
		InitializeComponent();
        BindingContext = listMqttActionViewModel;
        _listMqttActionViewModel = listMqttActionViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _listMqttActionViewModel.Initialize().ConfigureAwait(false);
    }
}