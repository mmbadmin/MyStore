namespace MyStore.Application.Users.Commands.Deactive
{
    using FluentValidation;

    public class UserDeactiveCommandValidator : AbstractValidator<UserDeactiveCommand>
    {
        public UserDeactiveCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
