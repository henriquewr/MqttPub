namespace MqttPub.Domain.Entities
{
    public class AppActionMqttAction : IEntity
    {
        public int Id { get; set; }
        public int Order { get; set; }

        public MqttAction MqttAction { get; set; } = null!;
        public int MqttActionId { get; set; }

        public AppActionEntity AppAction { get; set; } = null!;
        public int AppActionId { get; set; }
    }
}