namespace MyStore.Application.Users.Queries.GetData
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

    [AppAccess(nameof(User), "Users", "Get Information User", AllAccess = true)]
    public class UserGetDataQueryHandler : IRequestHandler<UserGetDataQuery, UserGetDataViewModel>
    {
        private readonly IBaseRepository<User, Guid> userRepository;
        private readonly IBaseRepository<Role, Guid> roleRepository;
        private readonly IBaseRepository<Permission, int> permissionRepository;

        public UserGetDataQueryHandler(IBaseRepository<User, Guid> userRepository,
                                       IBaseRepository<Role, Guid> roleRepository,
                                       IBaseRepository<Permission, int> permissionRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
        }

        public async Task<UserGetDataViewModel> Handle(UserGetDataQuery request, CancellationToken cancellationToken)
        {
            var item = await userRepository.FindAsync(predicate: x => x.Id == request.UserId,
                                                      selector: x => new
                                                      {
                                                          x.FirstName,
                                                          x.LastName,
                                                          x.UserName,
                                                          Roles = x.UserRoles.Select(x => x.RoleId)
                                                                             .ToList(),
                                                      });

            var roles = await roleRepository.ListAsync(predicate: x => item.Roles.Contains(x.Id),
                                                       includes: new List<Expression<Func<Role, object>>>
                                                       {
                                                           x => x.RolePermissions,
                                                       });

            var superUserId = await roleRepository.FindAsync(predicate: x => x.IsSystemAdmin, selector: x => x.Id);
            List<string> permissions;

            if (item.Roles.Any(x => x == superUserId))
            {
                permissions = await permissionRepository.ListAsync(selector: x => x.Id.ToString());
            }
            else
            {
                permissions = roles.SelectMany(x => x.RolePermissions)
                                       .Select(x => x.PermissionId.ToString()).ToList();
            }

            return new UserGetDataViewModel
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                Permissions = permissions,
            };
        }
    }
}
