namespace MyStore.Common.API.Mvc
{
    using MyStore.Common.Application.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using RExtension;
    using System;
    using System.Linq;
    using System.Security.Claims;

    public class CurrentUserInfo : ICurrentUserInfo
    {
        private readonly IServiceProvider provider;

        public CurrentUserInfo(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public Guid? UserId
        {
            get
            {
                var accessor = provider.GetService<IHttpContextAccessor>();
                var user = accessor?.HttpContext?.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    if (userId != null)
                    {
                        return userId.Value.ToGuid();
                    }
                }
                return null;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                var accessor = provider.GetService<IHttpContextAccessor>();
                var user = accessor?.HttpContext?.User;
                if (user != null)
                {
                    return user.Identity.IsAuthenticated;
                }
                return false;
            }
        }

        public string? ConnectionInfo
        {
            get
            {
                var accessor = provider.GetService<IHttpContextAccessor>();
                var httpContext = accessor?.HttpContext;
                if (httpContext != null)
                {
                    return httpContext?.Connection?.RemoteIpAddress?.ToString();
                }
                return null;
            }
        }

        public string? SessionKey
        {
            get
            {
                var accessor = provider.GetService<IHttpContextAccessor>();
                var user = accessor?.HttpContext?.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    var userData = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
                    if (userData != null)
                    {
                        return userData.Value;
                    }
                }
                return null;
            }
        }
    }
}
