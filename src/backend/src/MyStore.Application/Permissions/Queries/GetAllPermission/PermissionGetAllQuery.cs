namespace MyStore.Application.Permissions.Queries.GetAllPermission
{
    using MediatR;
    using System.Collections.Generic;

    public class PermissionGetAllQuery : IRequest<Dictionary<string, string>>
    {
    }
}
