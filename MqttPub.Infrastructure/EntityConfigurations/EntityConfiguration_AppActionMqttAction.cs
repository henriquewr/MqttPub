using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MqttPub.Domain.Entities;

namespace MqttPub.Infrastructure.EntityConfigurations
{
    internal class EntityConfiguration_AppActionMqttAction : IEntityTypeConfiguration<AppActionMqttAction>
    {
        public void Configure(EntityTypeBuilder<AppActionMqttAction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.MqttAction)
                .WithMany()
                .HasForeignKey(x => x.MqttActionId)
                .IsRequired();

            builder.HasOne(x => x.AppAction)
                .WithMany(x => x.MqttActions)
                .HasForeignKey(x => x.AppActionId)
                .IsRequired();
        }
    }
}
