namespace MyStore.Application.Roles.Commands.Delete
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Delete")]
    public class RoleDeleteCommandHandler : IRequestHandler<RoleDeleteCommand, bool>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleDeleteCommandHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            return await roleRepository.DeleteAsync(request.Id);
        }
    }
}
