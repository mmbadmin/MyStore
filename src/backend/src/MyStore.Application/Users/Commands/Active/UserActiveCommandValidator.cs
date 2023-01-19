namespace MyStore.Application.Users.Commands.Active
{
    using FluentValidation;

    public class UserActiveCommandValidator : AbstractValidator<UserActiveCommand>
    {
        public UserActiveCommandValidator()
        {
            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}
