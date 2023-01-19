namespace MyStore.Infrastructure.Persistence.Configs
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MyStore.Domain.Entities;

    public class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(nameof(RolePermission));

            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.HasOne<Role>()
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.RoleId)
                   .HasConstraintName($"FK_{nameof(RolePermission)}_{nameof(Role)}")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Permission>()
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.PermissionId)
                   .HasConstraintName($"FK_{nameof(RolePermission)}_{nameof(Permission)}")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
