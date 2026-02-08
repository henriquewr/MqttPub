using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MqttPub.Domain.Entities;

namespace MqttPub.Infrastructure.EntityConfigurations
{
    internal class EntityConfiguration_AppActionEntity : IEntityTypeConfiguration<AppActionEntity>
    {
        public void Configure(EntityTypeBuilder<AppActionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
