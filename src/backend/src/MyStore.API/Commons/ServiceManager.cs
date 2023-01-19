namespace MyStore.API.Commons
{
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MyStore.API.Jobs;
    using MyStore.Application.Commons;
    using MyStore.Common.API.Mvc;
    using MyStore.Common.Application.Behaviors;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Infrastructure.Persistence;

    public static class ServiceManager
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(Texts).Assembly;

            services.AddMediatR(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestAuthorizationPipeline<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestStringCleanBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IAuthorizationProvider, AuthorizationProvider>();
            services.AddScoped<IPermissionProvider, PermissionProvider>();

            services.AddSingleton<IHostedService, ServicePermissionJob>();

            services.Scan(scan => scan.FromAssemblies(assembly)
                    .AddClasses(x => x.AssignableTo(typeof(AbstractValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SLADbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"),
                                                 b => b.MigrationsAssembly(typeof(SLADbContext).Assembly.FullName)));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(EFRepository<,>));

            return services;
        }
    }
}
