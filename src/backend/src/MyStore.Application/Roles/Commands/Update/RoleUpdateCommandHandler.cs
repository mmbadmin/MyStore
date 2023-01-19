namespace MyStore.Application.Roles.Commands.Update
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(Role), "Role", "Edit")]
    public class RoleUpdateCommandHandler : IRequestHandler<RoleUpdateCommand, bool>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleUpdateCommandHandler(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var item = await roleRepository.FindAsync(predicate: x => x.Id == request.Id);
            if (item == null)
            {
                throw BaseException.NotFound(nameof(Role), request.Id);
            }
            item.UpdateTitle(request.Title);
            item.UpdateDescription(request.Description);
            return await roleRepository.UpdateAsync(item);
        }
    }
}
