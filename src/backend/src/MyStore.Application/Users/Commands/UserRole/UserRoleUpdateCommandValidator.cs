namespace MyStore.Application.Users.Commands.UserRole
{
    using FluentValidation;

    public class UserRoleUpdateCommandValidator : AbstractValidator<UserRoleUpdateCommand>
    {
        public UserRoleUpdateCommandValidator()
        {
            RuleFor(v => v.UserId).NotEmpty();
            RuleFor(v => v.RoleIds).NotNull();
        }
    }
}
