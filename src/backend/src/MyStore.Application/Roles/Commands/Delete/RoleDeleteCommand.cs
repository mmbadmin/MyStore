namespace MyStore.Application.Roles.Commands.Delete
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoleDeleteCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
