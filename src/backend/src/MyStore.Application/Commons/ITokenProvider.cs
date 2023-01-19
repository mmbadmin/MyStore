namespace MyStore.Application.Commons
{
    using System;

    public interface ITokenProvider
    {
        string CreateToken(Guid userId, string sessionKey);
    }
}
