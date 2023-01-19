namespace MyStore.Application.Users.Commands.Unlock
{
    using FluentValidation;

    public class UserUnlockCommandValidator : AbstractValidator<UserUnlockCommand>
    {
        public UserUnlockCommandValidator()
        {
            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}
