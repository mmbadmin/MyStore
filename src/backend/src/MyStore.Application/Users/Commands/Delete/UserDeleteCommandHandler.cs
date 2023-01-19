namespace MyStore.Application.Users.Commands.Delete
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "User", "Delete")]
    public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserDeleteCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var item = await userRepository.FindAsync(predicate: x => x.Id == request.Id);
            if (item == null)
            {
                throw BaseException.NotFound();
            }
            return await userRepository.DeleteAsync(item);
        }
    }
}
