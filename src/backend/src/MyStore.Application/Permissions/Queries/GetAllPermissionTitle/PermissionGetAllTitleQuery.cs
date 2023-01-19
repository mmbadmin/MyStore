namespace MyStore.Application.Permissions.Queries.GetAllPermissionTitle
{
    using MediatR;
    using System.Collections.Generic;

    public class PermissionGetAllTitleQuery : IRequest<Dictionary<string, string>>
    {
    }
}
