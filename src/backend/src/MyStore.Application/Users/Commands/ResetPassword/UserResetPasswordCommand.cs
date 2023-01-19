namespace MyStore.Application.Users.Commands.ResetPassword
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserResetPasswordCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid Id { get; set; }

        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
