namespace MyStore.Common.Application.Interfaces
{
    using System.Collections.Generic;

    public interface IPagedList<T>
    {
        IEnumerable<T>? Data { get; set; }

        int Total { get; set; }
    }
}
