namespace MyStore.Application.Permissions.Queries.GetAllPermissionTitle
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Permission), "Permissions", "Get All Permission‌s", AllAccess = true, Ignore = true)]
    public class PermissionGetAllTitleQueryHandler : IRequestHandler<PermissionGetAllTitleQuery, Dictionary<string, string>>
    {
        private readonly IBaseRepository<Permission, int> permissionRepository;

        public PermissionGetAllTitleQueryHandler(IBaseRepository<Permission, int> permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        public async Task<Dictionary<string, string>> Handle(PermissionGetAllTitleQuery request, CancellationToken cancellationToken)
        {
            var list = await permissionRepository.ListAsync(selector: x => new { x.Title, GroupTitle = x.PermissionGroup.Title, x.Id });
            var dict = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dict.Add($"{item.GroupTitle}_{item.Title}", "string");
            }
            return dict.OrderBy(x => x.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }
    }
}
