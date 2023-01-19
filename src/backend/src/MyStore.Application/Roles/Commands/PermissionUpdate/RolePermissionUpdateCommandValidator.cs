namespace MyStore.Application.Roles.Commands.PermissionUpdate
{
    using FluentValidation;

    public class RolePermissionUpdateCommandValidator : AbstractValidator<RolePermissionUpdateCommand>
    {
        public RolePermissionUpdateCommandValidator()
        {
            RuleFor(v => v.RoleId).NotEmpty();
            RuleFor(v => v.PermissionIds).NotNull();
        }
    }
}
