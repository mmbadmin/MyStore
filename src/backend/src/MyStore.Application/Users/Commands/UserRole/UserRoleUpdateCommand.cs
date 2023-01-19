namespace MyStore.Application.Users.Commands.UserRole
{
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserRoleUpdateCommand : IRequest<bool>
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }

        [Display(Name = "Role")]
        public List<Guid> RoleIds { get; set; }
    }
}
