namespace MyStore.Application.Users.Queries.GetPagedList
{
    using MediatR;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;

    public class UserGetPagedListQuery : IRequest<IPagedList<UserGetPagedListViewModel>>
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public FilterCollection Filters { get; set; }
    }
}
