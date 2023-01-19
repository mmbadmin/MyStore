namespace MyStore.Application.Roles.Commands.Create
{
    using FluentValidation;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Utilities;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleCreateCommandValidator(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
            RuleFor(v => v.Title).NotEmpty()
                                 .MaximumLength(255)
                                 .MustAsync(IsTitleUnique).WithMessage(Texts.Command.IsTitleUnique)
                                 .MaximumLength(50);
            RuleFor(v => v.Description).MaximumLength(255);
        }

        public async Task<bool> IsTitleUnique(string title, CancellationToken cancellationToken)
        {
            return !(await roleRepository.ListAsync(selector: x => x.Title)).CheckDuplicate(title);
        }
    }
}
