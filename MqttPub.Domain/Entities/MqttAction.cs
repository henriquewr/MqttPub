using System.Collections.Generic;

namespace MqttPub.Domain.Entities
{
    public class MqttAction : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public MqttConnection MqttConnection { get; set; } = null!;

        public int MqttConnectionId { get; set; }

        public ICollection<MqttMessage> MqttMessages { get; set; } = null!;
    }
}