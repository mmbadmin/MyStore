namespace MyStore.Application.Permissions.Queries.GetAllPermission
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Permission), "Permission s", "Get All Permission‌s", AllAccess = true, Ignore = true)]
    public class PermissionGetAllQueryHandler : IRequestHandler<PermissionGetAllQuery, Dictionary<string, string>>
    {
        private readonly IBaseRepository<Permission, int> permissionRepository;

        public PermissionGetAllQueryHandler(IBaseRepository<Permission, int> permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        public async Task<Dictionary<string, string>> Handle(PermissionGetAllQuery request, CancellationToken cancellationToken)
        {
            var list = await permissionRepository.ListAsync(selector: x => new { x.Title, GroupTitle = x.PermissionGroup.Title, x.Id });
            var dict = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dict.Add($"{item.GroupTitle}_{item.Title}", $"{item.Id}");
            }
            return dict;
        }
    }
}
