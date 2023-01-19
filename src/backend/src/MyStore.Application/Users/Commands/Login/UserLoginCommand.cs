namespace MyStore.Application.Users.Commands.Login
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class UserLoginCommand : IRequest<UserLoginViewModel>
    {
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Captcha Code")]
        public string Captcha { get; set; }
    }
}
