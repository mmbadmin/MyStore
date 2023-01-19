namespace MyStore.Application.Roles.Queries.GetList
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Get List Role", AllAccess = true)]
    public class RoleGetListQueryHandler : IRequestHandler<RoleGetListQuery, List<RoleGetListViewModel>>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleGetListQueryHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public Task<List<RoleGetListViewModel>> Handle(RoleGetListQuery request, CancellationToken cancellationToken)
        {
            return roleRepository.ListAsync(selector: x => new RoleGetListViewModel
            {
                Id = x.Id,
                Title = x.Title,
            });
        }
    }
}
