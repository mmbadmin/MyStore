namespace MyStore.Application.Users.Commands.ChangePassword
{
    using FluentValidation;
    using MyStore.Application.Commons;

    public class UserChangePasswordCommandValidator : AbstractValidator<UserChangePasswordCommand>
    {
        public UserChangePasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(Texts.Users.OldPasswordIsRequired);
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(Texts.Users.NewPasswordIsRequired)
                                       .MinimumLength(8).WithMessage(Texts.Users.PasswordCannotBeLessThanEightLetter);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.NewPassword).WithMessage(Texts.Users.NewPasswordAndConfirmPasswordInconsistent);
        }
    }
}
