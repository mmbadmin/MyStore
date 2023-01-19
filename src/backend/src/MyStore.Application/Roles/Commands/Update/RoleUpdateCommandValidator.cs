namespace MyStore.Application.Roles.Commands.Update
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Utilities;
    using MyStore.Application.Commons;
    using MyStore.Domain.Entities;
    using FluentValidation;
    using System;
    using System.Threading.Tasks;

    public class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
    {
        private readonly IBaseRepository<Role, Guid> roleRepository;

        public RoleUpdateCommandValidator(IBaseRepository<Role, Guid> roleRepository)
        {
            this.roleRepository = roleRepository;
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.Title).NotEmpty()
                                 .MaximumLength(255)
                                 .MustAsync((item, title, _) => IsTitleUnique(item.Id, item.Title))
                                 .WithMessage(Texts.Command.IsTitleUnique)
                                 .MaximumLength(50);
            RuleFor(v => v.Description).MaximumLength(255);
        }

        public async Task<bool> IsTitleUnique(Guid id, string title)
        {
            return !(await roleRepository.ListAsync(selector: x => x.Title,
                                                    predicate: x => x.Title == title && x.Id != id)).CheckDuplicate(title);
        }
    }
}
