namespace MyStore.Application.Users.Queries.GetItem
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Get details")]
    public class UserGetItemQueryHandler : IRequestHandler<UserGetItemQuery, UserGetItemViewModel>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserGetItemQueryHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserGetItemViewModel> Handle(UserGetItemQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.FindAsync(predicate: x => x.Id == request.Id, selector: x => new UserGetItemViewModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
            });
            if (result == null)
            {
                throw BaseException.NotFound();
            }
            return result;
        }
    }
}
