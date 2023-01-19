namespace MyStore.Common.Application.Behaviors
{
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;
    using MyStore.Common.Application.Interfaces;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserInfo currentUserInfo;

        public RequestLogger(ILogger<TRequest> logger, ICurrentUserInfo currentUserInfo)
        {
            this.logger = logger;
            this.currentUserInfo = currentUserInfo;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserInfo.UserId;

            logger.LogInformation("Request: {Name} {@UserId} {@Request}",
                                  requestName,
                                  userId,
                                  request);
            return Task.CompletedTask;
        }
    }
}
