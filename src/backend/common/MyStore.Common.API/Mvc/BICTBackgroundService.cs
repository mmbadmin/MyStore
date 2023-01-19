namespace MyStore.Common.API.Mvc
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class MyStoreBackgroundService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource stoppingCts = new CancellationTokenSource();

        private Task? executingTask;

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            executingTask = ExecuteAsync(stoppingCts.Token);

            if (executingTask?.IsCompleted == true)
            {
                return executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (executingTask == null)
            {
                return;
            }

            try
            {
                stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            stoppingCts.Cancel();
            stoppingCts.Dispose();
        }
    }
}
