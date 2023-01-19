namespace MyStore.Application.Users.Commands.Deactive
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "User", "Deactive")]
    public class UserDeactiveCommandHandler : IRequestHandler<UserDeactiveCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserDeactiveCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserDeactiveCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.UserId);
            if (user is null)
            {
                throw BaseException.NotFound();
            }
            user.Deactive();
            return await userRepository.UpdateAsync(user);
        }
    }
}
