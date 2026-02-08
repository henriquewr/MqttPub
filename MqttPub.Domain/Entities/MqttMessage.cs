namespace MqttPub.Domain.Entities
{
    public class MqttMessage : IEntity
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public required int Order { get; set; }

        public MqttAction MqttAction { get; set; } = null!;
        public int MqttActionId { get; set; }
    }
}