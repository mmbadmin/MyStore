namespace MyStore.Infrastructure.Persistence.Configs
{
    using MyStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserSessionConfig : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable(nameof(UserSession));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SessionKey).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ExpireDate).HasColumnType("dateTime");
            builder.Property(x => x.ConnectionInfo).HasMaxLength(200);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName($"FK_{nameof(UserSession)}_{nameof(User)}");
        }
    }
}
