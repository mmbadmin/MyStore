namespace MyStore.Application.Users.Commands.Delete
{
    using FluentValidation;

    public class UserDeleteCommandValidator : AbstractValidator<UserDeleteCommand>
    {
        public UserDeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
