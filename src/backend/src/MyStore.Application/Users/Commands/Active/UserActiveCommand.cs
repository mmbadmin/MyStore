namespace MyStore.Application.Users.Commands.Active
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserActiveCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }
    }
}
