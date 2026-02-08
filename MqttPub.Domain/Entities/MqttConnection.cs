using System.Collections.Generic;

namespace MqttPub.Domain.Entities
{
    public class MqttConnection : IEntity
    {
        public int Id { get; set; }
        public required string BrokerAddress { get; set; }
        public required string Topic { get; set; }
        public required string ClientId { get; set; }
        public required int Port { get; set; }

        public ICollection<MqttAction> MqttActions { get; set; } = null!;

        public string Name => $"{BrokerAddress}, {Topic}";
    }
}