namespace MyStore.Common.Application.Behaviors
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequestAuthorizationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserInfo currentUserInfo;
        private readonly IAuthorizationProvider authorizationProvider;

        public RequestAuthorizationPipeline(ICurrentUserInfo currentUserInfo,
                                            IAuthorizationProvider authorizationProvider)
        {
            this.currentUserInfo = currentUserInfo;
            this.authorizationProvider = authorizationProvider;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            CancellationToken cancellationToken,
                                            RequestHandlerDelegate<TResponse> next)
        {
            await CheckAccess();
            return await next();
        }

        private async Task CheckAccess()
        {
            var baseType = typeof(IRequestHandler<TRequest, TResponse>);
            var handler = typeof(TRequest).Assembly
                .GetTypes()
                .Where(p => baseType.IsAssignableFrom(p))
                .FirstOrDefault();
            if (!(handler.GetCustomAttributes(typeof(AppAccessAttribute), true).FirstOrDefault() is AppAccessAttribute attr))
            {
                throw new Exception(Const.RequestAuthorizationPipelineText.UndefinedAccess);
            }

            var groupName = attr.GroupName;
            var groupTitle = attr.GroupTitle;
            var handlerName = attr.Name;
            var handlerTittle = handler.Name;
            if (attr.Ignore)
            {
                return;
            }

            // db check
            var userId = currentUserInfo.UserId;
            var sessionKey = currentUserInfo.SessionKey;
            if (!currentUserInfo.IsAuthenticated || userId is null || sessionKey is null)
            {
                throw BaseException.UnAuthorize();
            }
            var userSessionCheck = await authorizationProvider.CheckUserSessionAsync(userId.Value, sessionKey);
            if (!userSessionCheck)
            {
                throw BaseException.UnAuthorize();
            }
            if (attr.AllAccess)
            {
                return;
            }
            var havePermission = await authorizationProvider.CheckUserPermissionAsync(userId.Value, handlerTittle);
            if (!havePermission)
            {
                throw BaseException.Forbidden();
            }
        }
    }
}
