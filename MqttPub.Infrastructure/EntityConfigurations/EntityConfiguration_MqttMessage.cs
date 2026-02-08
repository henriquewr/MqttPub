using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MqttPub.Domain.Entities;

namespace MqttPub.Infrastructure.EntityConfigurations
{
    internal class EntityConfiguration_MqttMessage : IEntityTypeConfiguration<MqttMessage>
    {
        public void Configure(EntityTypeBuilder<MqttMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.MqttAction)
                .WithMany(x => x.MqttMessages)
                .HasForeignKey(x => x.MqttActionId)
                .IsRequired();
        }
    }
}
