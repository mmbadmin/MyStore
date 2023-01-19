namespace MyStore.Application.Roles.Queries.GetItem
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Get Role")]
    public class RoleGetItemQueryHandler : IRequestHandler<RoleGetItemQuery, RoleGetItemViewModel>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleGetItemQueryHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<RoleGetItemViewModel> Handle(RoleGetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await roleRepository.FindAsync(predicate: x => x.Id == request.Id,
                                                      selector: x => new RoleGetItemViewModel
                                                      {
                                                          Id = x.Id,
                                                          Description = x.Description,
                                                          Title = x.Title,
                                                      });
            if (item is null)
            {
                throw BaseException.NotFound(nameof(Role), request.Id);
            }
            return item;
        }
    }
}
