namespace MyStore.Common.Application.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IAuthorizationProvider
    {
        Task<bool> CheckUserSessionAsync(Guid userId, string sessionKey);

        Task<bool> CheckUserPermissionAsync(Guid userId, string handlerTittle);
    }
}
