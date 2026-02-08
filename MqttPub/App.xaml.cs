#if ANDROID
using Android.Content;
using MqttPub.Platforms.Android.ForegroundServices;
#endif
using MqttPub.Pages.MqttConnection;
using MqttPub.Pages.MqttAction;
using MqttPub.Pages.AppAction;

namespace MqttPub
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();

            AppActions.OnAppAction += OnAppAction;

            Routing.RegisterRoute(nameof(ListMqttConnectionPage), typeof(ListMqttConnectionPage));
            Routing.RegisterRoute(nameof(CreateMqttConnectionPage), typeof(CreateMqttConnectionPage));

            Routing.RegisterRoute(nameof(ListMqttActionPage), typeof(ListMqttActionPage));
            Routing.RegisterRoute(nameof(CreateMqttActionPage), typeof(CreateMqttActionPage));

            Routing.RegisterRoute(nameof(ListAppActionPage), typeof(ListAppActionPage));
            Routing.RegisterRoute(nameof(CreateAppActionPage), typeof(CreateAppActionPage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }


        private void OnAppAction(object? sender, AppActionEventArgs e)
        {
#if ANDROID
            var activity = Platform.CurrentActivity;
            if (activity is not null)
            {
                activity.MoveTaskToBack(true);
                var intent = new Intent(activity, typeof(MqttForegroundService));
                intent.PutExtra("ActionId", int.Parse(e.AppAction.Id));

                if (OperatingSystem.IsAndroidVersionAtLeast(26))
                {
                    activity.StartForegroundService(intent);
                }
                else
                {
                    activity.StartService(intent);
                }
            }
#endif
        }
    }
}