namespace MyStore.Application.Users.Queries.GetRoles
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Get Roles")]
    public class UserGetRoleQueryHandler : IRequestHandler<UserGetRoleQuery, List<Guid>>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserGetRoleQueryHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<List<Guid>> Handle(UserGetRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.FindAsync(predicate: x => x.Id == request.Id,
                                                        includes: new List<Expression<Func<User, object>>> { x => x.UserRoles });
            if (result == null)
            {
                throw BaseException.NotFound();
            }
            return result.UserRoles.Select(x => x.RoleId).ToList();
        }
    }
}
