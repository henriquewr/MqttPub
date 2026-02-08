using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using MqttPub.Application.Services.MqttPub.Abstractions;

namespace MqttPub.Platforms.Android.ForegroundServices
{
    [Service(ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeDataSync)]
    public class MqttForegroundService : Service
    {
        private const int ServiceId = 10000;
        private IMqttPublisher _mqttPublisher = null!;
        public override void OnCreate()
        {
            base.OnCreate();
                
            _mqttPublisher = IPlatformApplication.Current!.Services.GetRequiredService<IMqttPublisher>();
        }

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            if (intent is not null)
            {
                var actionId = intent.GetIntExtra("ActionId", 0);
                if (actionId != 0)
                {
                    StartForeground(ServiceId, BuildNotification());

                    Task.Run(async () =>
                    {
                        try
                        {
                            await _mqttPublisher.RunAction(actionId);
                        }
                        catch(Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            StopSelf();
                        }
                    });
                }
            }

            return StartCommandResult.Sticky;
        }

        private Notification BuildNotification()
        {
            var channelId = "mqtt_service_channel";

            if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                var manager = (NotificationManager?)GetSystemService(NotificationService);
                if (manager is not null)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "MQTT Service",
                        NotificationImportance.Default
                    );
                    manager.CreateNotificationChannel(channel);
                }
            }

            var builder = new NotificationCompat.Builder(this, channelId)!
                .SetContentTitle("MQTT rodando")!
                .SetContentText("Seu app está conectado ao broker.")!
                .SetSmallIcon(Resource.Drawable.ic_call_answer)!
                .SetOngoing(true)!;

            return builder.Build()!;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override IBinder? OnBind(Intent? intent)
        {
            throw new NotImplementedException();
        }
    }
}
