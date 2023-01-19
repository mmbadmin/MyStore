namespace MyStore.Application.Roles.Queries.GetPermissions
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    public class RoleGetPermissionListQuery : IRequest<List<int>>
    {
        public Guid Id { get; set; }
    }
}
