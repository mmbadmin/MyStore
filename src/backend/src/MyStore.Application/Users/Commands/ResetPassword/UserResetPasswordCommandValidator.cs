namespace MyStore.Application.Users.Commands.ResetPassword
{
    using MyStore.Application.Commons;
    using FluentValidation;

    public class UserResetPasswordCommandValidator : AbstractValidator<UserResetPasswordCommand>
    {
        public UserResetPasswordCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(Texts.Users.UserIsRequired);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Texts.Users.NewPasswordIsRequired)
                                    .MinimumLength(8).WithMessage(Texts.Users.PasswordCannotBeLessThanEightLetter);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Texts.Users.NewPasswordAndConfirmPasswordInconsistent);
        }
    }
}
