namespace MyStore.Infrastructure.Persistence.Configs
{
    using MyStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PermissionGroupConfig : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder.ToTable(nameof(PermissionGroup));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        }
    }
}
