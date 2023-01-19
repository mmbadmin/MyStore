namespace MyStore.Common.API.Mvc
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using RExtension;
    using System;
    using System.Linq;
    using System.Security.Claims;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseController : ControllerBase
    {
        private IMediator? mediator;

        public Guid UserId
        {
            get
            {
                var accessor = HttpContext.RequestServices.GetService<IHttpContextAccessor>();
                var user = accessor?.HttpContext?.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    if (userId != null)
                    {
                        return userId.Value.ToGuid();
                    }
                }
                return Guid.Empty;
            }
        }

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
