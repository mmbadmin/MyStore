namespace MyStore.Application.Users.Queries.GetData
{
    using MediatR;
    using System;

    public class UserGetDataQuery : IRequest<UserGetDataViewModel>
    {
        public Guid UserId { get; set; }
    }
}
