namespace MyStore.Common.Application.Interfaces
{
    using System;

    public interface ICurrentUserInfo
    {
        Guid? UserId { get; }

        bool IsAuthenticated { get; }

        string? ConnectionInfo { get; }

        string? SessionKey { get; }
    }
}
