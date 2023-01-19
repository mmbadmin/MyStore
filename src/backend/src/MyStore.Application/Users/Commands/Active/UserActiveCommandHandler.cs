namespace MyStore.Application.Users.Commands.Active
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Enable user")]
    public class UserActiveCommandHandler : IRequestHandler<UserActiveCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserActiveCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserActiveCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.UserId);
            if (user is null)
            {
                throw BaseException.NotFound();
            }
            user.Active();
            return await userRepository.UpdateAsync(user);
        }
    }
}
