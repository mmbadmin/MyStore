#nullable disable

namespace MyStore.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Infrastructure;
    using MyStore.Domain.Entities;

    public class SLADbContext : BaseDbContext
    {
        public SLADbContext(DbContextOptions<SLADbContext> options, ICurrentUserInfo currentUserInfo)
            : base(options, currentUserInfo)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<PermissionGroup> PermissionGroups { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public virtual DbSet<UserSession> UserSessions { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SLADbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
