namespace MyStore.Application.Roles.Commands.PermissionUpdate
{
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RolePermissionUpdateCommand : IRequest<bool>
    {
        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "Permissions")]
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
