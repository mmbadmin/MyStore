namespace MyStore.Application.Permissions.Queries.GetList
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Permission), "Permissions", "Get List", AllAccess = true)]
    public class PermissionGetListQueryHandler : IRequestHandler<PermissionGetListQuery, List<PermissionGroupListViewModel>>
    {
        private readonly IBaseRepository<PermissionGroup, int> permissionGroupRepository;

        public PermissionGetListQueryHandler(IBaseRepository<PermissionGroup, int> permissionGroupRepository)
        {
            this.permissionGroupRepository = permissionGroupRepository;
        }

        public Task<List<PermissionGroupListViewModel>> Handle(PermissionGetListQuery request, CancellationToken cancellationToken)
        {
            return permissionGroupRepository.ListAsync(orderModels: OrderModel.GetOrderModels("Name", "asc"),
                                                       selector: x => new PermissionGroupListViewModel
                                                       {
                                                           Id = $"C{x.Id}",
                                                           Title = x.Name,
                                                           Permissions = x.Permissions.Select(z => new PermissionListViewModel
                                                           {
                                                               Id = z.Id,
                                                               Title = z.Name,
                                                           }).ToList(),
                                                       });
        }
    }
}
