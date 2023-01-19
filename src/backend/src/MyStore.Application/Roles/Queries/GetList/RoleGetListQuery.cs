namespace MyStore.Application.Roles.Queries.GetList
{
    using MediatR;
    using System.Collections.Generic;

    public class RoleGetListQuery : IRequest<List<RoleGetListViewModel>>
    {
    }
}
