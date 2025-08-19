using MqttPub.ViewModels.MqttActionModels.Create;

namespace MqttPub.Pages.MqttAction;

public partial class CreateMqttActionPage : ContentPage, IQueryAttributable
{
	public static string Url(int mqttActionId)
	{
		return $"{nameof(CreateMqttActionPage)}?{nameof(MqttActionId)}={mqttActionId}";
	}

    private readonly CreateMqttActionViewModel _createMqttActionViewModel;

    public int? MqttActionId { get; set; }
	public CreateMqttActionPage(CreateMqttActionViewModel createMqttActionViewModel)
    {
		InitializeComponent();
		BindingContext = createMqttActionViewModel;
        _createMqttActionViewModel = createMqttActionViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _createMqttActionViewModel.InitializeAsync().ConfigureAwait(false);
        if (MqttActionId.HasValue)
        {
            await _createMqttActionViewModel.LoadMqttAction(MqttActionId.Value).ConfigureAwait(false);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(MqttActionId), out var value) && value is string idStr && int.TryParse(idStr, out var parsed))
        {
            MqttActionId = parsed;
        }
    }
}