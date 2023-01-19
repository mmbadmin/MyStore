namespace MyStore.Application.Roles.Queries.GetPermissions
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Get Permission Role", AllAccess = true, Ignore = true)]
    public class RoleGetPermissionListQueryHandler : IRequestHandler<RoleGetPermissionListQuery, List<int>>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleGetPermissionListQueryHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<List<int>> Handle(RoleGetPermissionListQuery request, CancellationToken cancellationToken)
        {
            var item = await roleRepository.FindAsync(predicate: x => x.Id == request.Id,
                                                      selector: x => x,
                                                      includes: new List<Expression<Func<Role, object>>> { x => x.RolePermissions });

            return item.RolePermissions.Select(x => x.PermissionId).ToList();
        }
    }
}
