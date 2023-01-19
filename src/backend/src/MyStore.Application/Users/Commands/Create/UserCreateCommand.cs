namespace MyStore.Application.Users.Commands.Create
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class UserCreateCommand : IRequest<bool>
    {
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
