namespace MyStore.API.Commons
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly IBaseRepository<User, Guid> userRepository;
        private readonly IBaseRepository<Role, Guid> roleRepository;
        private readonly IBaseRepository<Permission, int> permissionRepository;
        private readonly IBaseRepository<UserSession, int> userSessionRepository;

        public AuthorizationProvider(IBaseRepository<User, Guid> userRepository,
                                     IBaseRepository<Role, Guid> roleRepository,
                                     IBaseRepository<Permission, int> permissionRepository,
                                     IBaseRepository<UserSession, int> userSessionRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.userSessionRepository = userSessionRepository;
        }

        public async Task<bool> CheckUserPermissionAsync(Guid userId, string handlerTittle)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == userId,
                                                      includes: new List<Expression<Func<User, object>>>
                                                      {
                                                          x => x.UserRoles,
                                                      });
            var userRole = await roleRepository.ListAsync(predicate: x => user.UserRoles.Select(z => z.RoleId).Contains(x.Id));
            if (userRole.Any(x => x.IsSystemAdmin))
            {
                return true;
            }

            var permission = await permissionRepository.FindAsync(predicate: x => x.Title.ToLower() == handlerTittle.ToLower());
            if (permission is null)
            {
                // log here
                return false;
            }

            var rolePermission = await roleRepository.ListAsync(predicate: x => x.RolePermissions.Any(z => z.PermissionId == permission.Id),
                                                                selector: x => x.Id);
            if (userRole.Any(x => rolePermission.Contains(x.Id)))
            {
                return true;
            }
            return false;
        }

        public Task<bool> CheckUserSessionAsync(Guid userId, string sessionKey)
        {
            return userSessionRepository.AnyAsync(x => x.UserId == userId &&
                                                       x.SessionKey == sessionKey &&
                                                       x.ExpireDate >= DateTime.Now &&
                                                       !x.IsExpired);
        }
    }
}
