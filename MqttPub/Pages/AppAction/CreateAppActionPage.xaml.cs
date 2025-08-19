using MqttPub.ViewModels.AppActionModels.Create;

namespace MqttPub.Pages.AppAction;

public partial class CreateAppActionPage : ContentPage, IQueryAttributable
{
    public static string Url(int appActionId)
    {
        return $"{nameof(CreateAppActionPage)}?{nameof(AppActionId)}={appActionId}";
    }

    private readonly CreateAppActionViewModel _createAppActionViewModel;

    public int? AppActionId { get; set; }
    public CreateAppActionPage(CreateAppActionViewModel createAppActionViewModel)
    {
        InitializeComponent();
        BindingContext = createAppActionViewModel;
        _createAppActionViewModel = createAppActionViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _createAppActionViewModel.InitializeAsync().ConfigureAwait(false);
        if (AppActionId.HasValue)
        {
            await _createAppActionViewModel.LoadAppAction(AppActionId.Value).ConfigureAwait(false);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(AppActionId), out var value) && value is string idStr && int.TryParse(idStr, out var parsed))
        {
            AppActionId = parsed;
        }
    }
}