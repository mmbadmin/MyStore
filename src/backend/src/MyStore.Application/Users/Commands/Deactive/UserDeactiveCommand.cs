namespace MyStore.Application.Users.Commands.Deactive
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDeactiveCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }
    }
}
