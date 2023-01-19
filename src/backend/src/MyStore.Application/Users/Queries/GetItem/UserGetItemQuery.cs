namespace MyStore.Application.Users.Queries.GetItem
{
    using MediatR;
    using System;

    public class UserGetItemQuery : IRequest<UserGetItemViewModel>
    {
        public Guid Id { get; set; }
    }
}
