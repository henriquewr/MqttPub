using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MqttPub.Domain.Entities;

namespace MqttPub.Infrastructure.EntityConfigurations
{
    internal class EntityConfiguration_MqttAction : IEntityTypeConfiguration<MqttAction>
    {
        public void Configure(EntityTypeBuilder<MqttAction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasOne(x => x.MqttConnection)
                .WithMany(x => x.MqttActions)
                .HasForeignKey(x => x.MqttConnectionId)
                .IsRequired();

            builder.HasMany(x => x.MqttMessages)
                .WithOne(x => x.MqttAction)
                .HasForeignKey(x => x.MqttActionId)
                .IsRequired();
        }
    }
}
