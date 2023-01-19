namespace MyStore.Application.Users.Commands.Update
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserUpdateCommand : IRequest<bool>
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
