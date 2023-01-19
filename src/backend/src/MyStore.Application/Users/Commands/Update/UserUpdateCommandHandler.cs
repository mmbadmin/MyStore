namespace MyStore.Application.Users.Commands.Update
{
    using MediatR;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Edit")]
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserUpdateCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var item = await userRepository.FindAsync(predicate: x => x.Id == request.Id);
            if (item == null)
            {
                throw BaseException.NotFound();
            }
            item.UpdateFirstName(request.FirstName);
            item.UpdateLastName(request.LastName);
            return await userRepository.UpdateAsync(item);
        }
    }
}
