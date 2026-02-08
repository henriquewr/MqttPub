using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MqttPub.Domain.Entities;

namespace MqttPub.Infrastructure.EntityConfigurations
{
    internal class EntityConfiguration_MqttConnection : IEntityTypeConfiguration<MqttConnection>
    {
        public void Configure(EntityTypeBuilder<MqttConnection> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.MqttActions)
                .WithOne(x => x.MqttConnection)
                .HasForeignKey(x => x.MqttConnectionId)
                .IsRequired();

            builder.Ignore(x => x.Name);
        }
    }
}
