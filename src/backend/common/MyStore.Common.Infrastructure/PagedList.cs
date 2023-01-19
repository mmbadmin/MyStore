namespace MyStore.Common.Infrastructure
{
    using MyStore.Common.Application.Interfaces;
    using System.Collections.Generic;

    public class PagedList<T> : IPagedList<T>
    {
        public int Total { get; set; }

        public IEnumerable<T>? Data { get; set; }
    }
}
