namespace MyStore.Application.Roles.Queries.GetItem
{
    using MediatR;
    using System;

    public class RoleGetItemQuery : IRequest<RoleGetItemViewModel>
    {
        public Guid Id { get; set; }
    }
}
