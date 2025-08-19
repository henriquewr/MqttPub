using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MqttPub.Data.Entities
{
    public class MqttMessage : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Message { get; set; }
        public required int Order { get; set; }


        [ForeignKey(nameof(MqttActionId))]
        public MqttAction MqttAction { get; set; } = null!;

        [ForeignKey(nameof(MqttAction))]
        public int MqttActionId { get; set; }
    }
}