namespace MyStore.Application.Roles.Commands.Create
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Create")]
    public class RoleCreateCommandHandler : IRequestHandler<RoleCreateCommand, bool>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleCreateCommandHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public Task<bool> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var item = new Role(Guid.NewGuid(), request.Title, request.Description);
            return roleRepository.AddAsync(item);
        }
    }
}
