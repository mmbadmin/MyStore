namespace MyStore.Infrastructure.Persistence.Configs
{
    using MyStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);

            builder.HasOne(x => x.PermissionGroup)
                   .WithMany(x => x.Permissions)
                   .HasForeignKey(x => x.PermissionGroupId)
                   .HasConstraintName($"FK_{nameof(Permission)}_{nameof(PermissionGroup)}")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
