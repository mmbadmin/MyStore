namespace MyStore.Common.Application.Behaviors
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using MyStore.Common.Application.Interfaces;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch timer;
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserInfo currentUserInfo;

        public RequestPerformanceBehavior(
            ILogger<TRequest> logger,
            ICurrentUserInfo currentUserInfo)
        {
            timer = new Stopwatch();

            this.logger = logger;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            CancellationToken cancellationToken,
                                            RequestHandlerDelegate<TResponse> next)
        {
            timer.Start();

            var response = await next();

            timer.Stop();

            var elapsedMilliseconds = timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = currentUserInfo.UserId;
                var userName = string.Empty;
                logger.LogWarning("MyStore Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                                  requestName,
                                  elapsedMilliseconds,
                                  userId,
                                  userName,
                                  request);
            }

            return response;
        }
    }
}
