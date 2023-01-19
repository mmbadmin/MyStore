namespace MyStore.Application.Users.Queries.GetRoles
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    public class UserGetRoleQuery : IRequest<List<Guid>>
    {
        public Guid Id { get; set; }
    }
}
