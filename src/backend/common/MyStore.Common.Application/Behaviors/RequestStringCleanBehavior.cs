namespace MyStore.Common.Application.Behaviors
{
    using FluentValidation;
    using MediatR;
    using RExtension;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequestStringCleanBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request,
                                      CancellationToken cancellationToken,
                                      RequestHandlerDelegate<TResponse> next)
        {
            var stringProperties = request.GetType()
                                          .GetProperties()
                                          .Where(p => p.PropertyType == typeof(string))
                                          .ToList();
            foreach (var stringProperty in stringProperties)
            {
                if (stringProperty.Name.ToLower() == "password")
                {
                    continue;
                }
                var currentValue = stringProperty.GetValue(request, null) as string;
                if (currentValue.IsEmpty())
                {
                    continue;
                }
            }

            return next();
        }
    }
}
