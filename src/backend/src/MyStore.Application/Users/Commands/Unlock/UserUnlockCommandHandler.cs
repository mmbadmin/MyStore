namespace MyStore.Application.Users.Commands.Unlock
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Unlock User")]
    public class UserUnlockCommandHandler : IRequestHandler<UserUnlockCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserUnlockCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserUnlockCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.UserId);
            if (user is null)
            {
                throw BaseException.NotFound();
            }
            user.Unlock();
            return await userRepository.UpdateAsync(user);
        }
    }
}
