namespace MyStore.Common.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Domain;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class BaseDbContext : DbContext
    {
        private readonly ICurrentUserInfo currentUserInfo;

        public const string IsDeleted = "IsDeleted";
        public const string CreatedDate = "CreatedDate";
        public const string ModifiedDate = "ModifiedDate";
        public const string CreatedBy = "CreatedBy";
        public const string ModifiedBy = "ModifiedBy";

        public BaseDbContext(DbContextOptions options, ICurrentUserInfo currentUserInfo)
            : base(options)
        {
            this.currentUserInfo = currentUserInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                                                         .Where(e => typeof(IAuditableEntity)
                                                         .IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime?>(CreatedDate)
                            .HasColumnType("datetime");

                modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime?>(ModifiedDate)
                            .HasColumnType("datetime");

                modelBuilder.Entity(entityType.ClrType)
                            .Property<Guid?>(CreatedBy);

                modelBuilder.Entity(entityType.ClrType)
                            .Property<Guid?>(ModifiedBy);
            }
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                                                         .Where(e => typeof(IDeletedEntity)
                                                         .IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                            .Property<bool>(IsDeleted);

                modelBuilder.Entity(entityType.ClrType)
                            .HasQueryFilter(ConvertFilterExpression<IDeletedEntity>(e => EF.Property<bool>(e, IsDeleted) == false, entityType.ClrType));
            }
        }

        protected void ApplyAuditInformation()
        {
            foreach (var entity in ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Added ||
                                                                                        e.State == EntityState.Modified))
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property("CreatedDate").CurrentValue = DateTime.Now;
                    entity.Property("CreatedBy").CurrentValue = currentUserInfo?.UserId;
                }
                else
                {
                    entity.Property("ModifiedDate").CurrentValue = DateTime.Now;
                    entity.Property("ModifiedBy").CurrentValue = currentUserInfo?.UserId;
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            var af = await base.SaveChangesAsync(cancellationToken);
            return af;
        }

        private static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression,
                                                                            Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(),
                                                             newParam,
                                                             filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}
