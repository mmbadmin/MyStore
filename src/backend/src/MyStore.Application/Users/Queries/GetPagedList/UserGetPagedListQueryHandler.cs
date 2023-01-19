namespace MyStore.Application.Users.Queries.GetPagedList
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", " Get List Users")]
    public class UserGetPagedListQueryHandler : IRequestHandler<UserGetPagedListQuery, IPagedList<UserGetPagedListViewModel>>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserGetPagedListQueryHandler(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<IPagedList<UserGetPagedListViewModel>> Handle(UserGetPagedListQuery request, CancellationToken cancellationToken)
        {
            return userRepository.PagedListAsync(page: request.Page,
                                                       pageSize: request.PageSize,
                                                       filters: request.Filters,
                                                       orderModels: OrderModel.GetOrderModels(request.SortColumn, request.SortOrder),
                                                       selector: x => new UserGetPagedListViewModel
                                                       {
                                                           Id = x.Id,
                                                           UserName = x.UserName,
                                                           FirstName = x.FirstName,
                                                           LastName = x.LastName,
                                                           IsActive = x.IsActive,
                                                           IsLocked = x.IsLocked,
                                                       });
        }
    }
}
