namespace MyStore.Common.Domain
{
    using System;

    public class BaseEntity<TKey>
        where TKey : struct, IComparable<TKey>, IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
