using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MqttPub.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class MqttAction : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        [ForeignKey(nameof(MqttConnectionId))]
        public MqttConnection MqttConnection { get; set; } = null!;

        [ForeignKey(nameof(MqttConnection))]
        public int MqttConnectionId { get; set; }

        public ICollection<MqttMessage> MqttMessages { get; set; } = null!;
    }
}