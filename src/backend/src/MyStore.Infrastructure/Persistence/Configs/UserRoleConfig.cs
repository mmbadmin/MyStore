namespace MyStore.Infrastructure.Persistence.Configs
{
    using MyStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole));
            builder.HasKey(x => new { x.RoleId, x.UserId });

            builder.HasOne<Role>()
                   .WithMany(x => x.UserRoles)
                   .HasForeignKey(x => x.RoleId)
                   .HasConstraintName($"FK_{nameof(UserRole)}_{nameof(Role)}")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany(x => x.UserRoles)
                   .HasForeignKey(x => x.UserId)
                   .HasConstraintName($"FK_{nameof(UserRole)}_{nameof(User)}")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
