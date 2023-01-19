namespace MyStore.Application.Permissions.Queries.GetList
{
    using MediatR;
    using System.Collections.Generic;

    public class PermissionGetListQuery : IRequest<List<PermissionGroupListViewModel>>
    {
    }
}
