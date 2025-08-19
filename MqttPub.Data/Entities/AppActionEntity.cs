using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MqttPub.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class AppActionEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<AppActionMqttAction> MqttActions { get; set; } = null!;
    }
}