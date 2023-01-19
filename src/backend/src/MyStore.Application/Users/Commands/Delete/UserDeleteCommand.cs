namespace MyStore.Application.Users.Commands.Delete
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDeleteCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid Id { get; set; }
    }
}
