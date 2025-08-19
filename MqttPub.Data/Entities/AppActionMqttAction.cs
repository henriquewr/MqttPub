using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MqttPub.Data.Entities
{
    public class AppActionMqttAction : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [ForeignKey(nameof(MqttActionId))]
        public MqttAction MqttAction { get; set; } = null!;

        [ForeignKey(nameof(MqttAction))]
        public int MqttActionId { get; set; }


        [ForeignKey(nameof(AppActionId))]
        public AppActionEntity AppAction { get; set; } = null!;

        [ForeignKey(nameof(AppAction))]
        public int AppActionId { get; set; }
    }
}