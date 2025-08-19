using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MqttPub.Data.Entities
{
    public class MqttConnection : IEntity
    {
        [Key]
        public int Id { get; set; }
        public required string BrokerAddress { get; set; }
        public required string Topic { get; set; }
        public required string ClientId { get; set; }
        public required int Port { get; set; }

        [NotMapped]
        public string Name => $"{BrokerAddress}, {Topic}";
    }
}