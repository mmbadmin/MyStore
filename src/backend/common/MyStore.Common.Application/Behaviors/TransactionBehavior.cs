namespace MyStore.Common.Application.Behaviors
{
    using MediatR;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;

    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
              public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var baseType = typeof(IRequestHandler<TRequest, TResponse>);
            var handler = typeof(TRequest).Assembly
                .GetTypes()
                .Where(p => baseType.IsAssignableFrom(p))
                .FirstOrDefault();
            if (handler.GetCustomAttributes(typeof(TransactionAttribute), true).FirstOrDefault() is TransactionAttribute attr)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.MaximumTimeout,
                };
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var response = await next();
                        transaction.Complete();
                        return response;
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        throw;
                    }
                }
            }
            else
            {
                var response = await next();
                return response;
            }
        }
    }
}
