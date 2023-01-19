namespace MyStore.Application.Roles.Queries.GetPagedList
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Get Paged List")]
    public class RoleGetPagedListQueryHandler : IRequestHandler<RoleGetPagedListQuery, IPagedList<RoleGetPagedListViewModel>>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleGetPagedListQueryHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public Task<IPagedList<RoleGetPagedListViewModel>> Handle(RoleGetPagedListQuery request, CancellationToken cancellationToken)
        {
            return roleRepository.PagedListAsync(page: request.Page,
                                                 pageSize: request.PageSize,
                                                 predicate: x => !x.IsSystemAdmin,
                                                 filters: request.Filters,
                                                 orderModels: OrderModel.GetOrderModels(request.SortColumn, request.SortOrder),
                                                 selector: x => new RoleGetPagedListViewModel
                                                 {
                                                     Description = x.Description,
                                                     Id = x.Id,
                                                     Title = x.Title,
                                                 });
        }
    }
}
