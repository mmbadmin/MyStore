namespace MyStore.Common.API.Models
{
    using MyStore.Common.Application.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using RExtension;

    public class PageQueryViewModel
    {
        [FromQuery(Name = "p")]
        public int Page { get; set; }

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; }

        [FromQuery(Name = "sc")]
        public string? SortColumn { get; set; }

        [FromQuery(Name = "so")]
        public string? SortOrder { get; set; }

        [FromQuery(Name = "filter")]
        public string? Filter { get; set; }

        public FilterCollection? Filters()
        {
            return Filter.IsEmpty() ? null : JsonConvert.DeserializeObject<FilterCollection>(Filter);
        }

        public override string ToString()
        {
            return $"Page={Page}, PageSize={PageSize}, SortColumn={SortColumn}, SortOrder={SortOrder}";
        }
    }
}
