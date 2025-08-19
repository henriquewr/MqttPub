using MqttPub.ViewModels.MqttConnectionModels.Create;

namespace MqttPub.Pages.MqttConnection;

public partial class CreateMqttConnectionPage : ContentPage, IQueryAttributable
{
	public static string Url(int mqttConnectionId)
	{
		return $"{nameof(CreateMqttConnectionPage)}?{nameof(MqttConnectionId)}={mqttConnectionId}";
	}

    private readonly CreateMqttConnectionViewModel _createMqttConnectionViewModel;

    public int? MqttConnectionId { get; set; }
	public CreateMqttConnectionPage(CreateMqttConnectionViewModel createMqttConnectionViewModel)
	{
		InitializeComponent();
		BindingContext = createMqttConnectionViewModel;
        _createMqttConnectionViewModel = createMqttConnectionViewModel;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(MqttConnectionId), out var value) && value is string idStr && int.TryParse(idStr, out var parsed))
        {
            MqttConnectionId = parsed;
            await _createMqttConnectionViewModel.LoadMqttConnection(MqttConnectionId.Value).ConfigureAwait(false);
        }
    }
}