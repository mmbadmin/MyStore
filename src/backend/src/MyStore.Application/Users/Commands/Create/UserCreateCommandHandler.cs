namespace MyStore.Application.Users.Commands.Create
{
    using MediatR;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Create")]
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserCreateCommandHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<bool> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var item = new User(Guid.NewGuid(),
                                request.UserName,
                                request.FirstName,
                                request.LastName,
                                PasswordHelper.Hash(request.Password));
            return userRepository.AddAsync(item);
        }
    }
}
