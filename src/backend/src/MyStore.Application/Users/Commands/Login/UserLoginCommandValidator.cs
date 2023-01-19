namespace MyStore.Application.Users.Commands.Login
{
    using FluentValidation;

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(v => v.UserName).NotEmpty();
            RuleFor(v => v.Password).NotEmpty();
            //RuleFor(v => v.Captcha).NotEmpty();
        }
    }
}
