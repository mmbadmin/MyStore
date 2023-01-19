namespace MyStore.Application.Users.Commands.Unlock
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserUnlockCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }
    }
}
