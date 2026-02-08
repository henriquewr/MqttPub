using System.Threading;
using System.Threading.Tasks;

namespace MqttPub.Application.Services.MqttPub.Abstractions
{
    public interface IMqttPublisher
    {
        Task RunAction(int appActionId, CancellationToken cancellationToken = default);
    }
}
