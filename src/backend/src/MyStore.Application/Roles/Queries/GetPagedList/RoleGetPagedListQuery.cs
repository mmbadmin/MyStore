namespace MyStore.Application.Roles.Queries.GetPagedList
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MediatR;

    public class RoleGetPagedListQuery : IRequest<IPagedList<RoleGetPagedListViewModel>>
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public FilterCollection Filters { get; set; }
    }
}
