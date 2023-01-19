namespace MyStore.Infrastructure.Persistence
{
    using MyStore.Common.Domain;
    using MyStore.Common.Infrastructure;
    using System;

    public class EFRepository<TEntity, TKey> : EFBaseRepository<SLADbContext, TEntity, TKey>, IDisposable
          where TEntity : BaseEntity<TKey>, IAggregateRoot
          where TKey : struct, IComparable<TKey>, IEquatable<TKey>
    {
        public EFRepository(SLADbContext db)
            : base(db)
        {
        }
    }
}
