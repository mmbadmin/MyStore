namespace MyStore.Common.API.Models
{
    using System.Collections.Generic;

    public class MyStoreResponse
    {
        public IEnumerable<string> Message { get; set; } = new List<string>();
    }
}
