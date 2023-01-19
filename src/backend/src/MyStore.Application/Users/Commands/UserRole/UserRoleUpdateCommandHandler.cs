namespace MyStore.Application.Users.Commands.UserRole
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

    [AppAccess(nameof(User), "Users", "Edit Role")]
    public class UserRoleUpdateCommandHandler : IRequestHandler<UserRoleUpdateCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserRoleUpdateCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.UserId,
                                                      includes: new List<Expression<Func<User, object>>>
                                                      {
                                                          x => x.UserRoles,
                                                      });
            user.ClearRoles();
            await userRepository.UpdateAsync(user);
            foreach (var permissionId in request.RoleIds)
            {
                user.AddRole(permissionId);
            }
            return await userRepository.UpdateAsync(user);
        }
    }
}
