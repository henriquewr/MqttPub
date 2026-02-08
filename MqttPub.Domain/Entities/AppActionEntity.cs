using System.Collections.Generic;

namespace MqttPub.Domain.Entities
{
    public class AppActionEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<AppActionMqttAction> MqttActions { get; set; } = null!;
    }
}