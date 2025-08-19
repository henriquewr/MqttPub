
namespace MqttPub.Services.MqttPub
{
    public interface IMqttPublisher
    {
        Task RunAction(int appActionId, CancellationToken cancellationToken = default);
    }
}
