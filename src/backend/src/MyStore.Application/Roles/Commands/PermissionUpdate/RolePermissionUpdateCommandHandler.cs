namespace MyStore.Application.Roles.Commands.PermissionUpdate
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Permission")]
    public class RolePermissionUpdateCommandHandler : IRequestHandler<RolePermissionUpdateCommand, bool>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RolePermissionUpdateCommandHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RolePermissionUpdateCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.FindAsync(predicate: x => x.Id == request.RoleId,
                                                      includes: new List<Expression<Func<Role, object>>> { x => x.RolePermissions });
            role.ClearPermissions();
            await roleRepository.UpdateAsync(role);
            foreach (var permissionId in request.PermissionIds)
            {
                role.AddPermission(permissionId);
            }
            return await roleRepository.UpdateAsync(role);
        }
    }
}
