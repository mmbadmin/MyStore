namespace MyStore.API.Jobs
{
    using MyStore.Common.API.Mvc;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Application.Commons;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ServicePermissionJob : MyStoreBackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ServicePermissionJob> logger;

        public ServicePermissionJob(IServiceProvider serviceProvider, ILogger<ServicePermissionJob> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Exec();
        }

        private Task Exec()
        {
            logger.LogInformation("Service Permission Job Start");
            using var scope = serviceProvider.CreateScope();
            var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
            var assembly = typeof(Texts).Assembly;
            var types = assembly.GetTypes()
                                .Where(x => x.GetCustomAttributes(typeof(AppAccessAttribute), true).Length > 0)
                                .ToList();
            var permitionGroupList = new List<PermissionGroupDataViewModel>();
            foreach (var item in types)
            {
                if (!(item.GetCustomAttributes(typeof(AppAccessAttribute), true).First() is AppAccessAttribute attr))
                {
                    continue;
                }
                var groupName = attr.GroupName;
                var groupTitle = attr.GroupTitle;
                var handlerName = attr.Name;
                var handlerTittle = item.Name;
                var pg = permitionGroupList.FirstOrDefault(x => x.Title == groupTitle);
                if (pg is null)
                {
                    pg = new PermissionGroupDataViewModel
                    {
                        Name = groupName,
                        Title = groupTitle,
                    };
                    permitionGroupList.Add(pg);
                }
                if (attr.Ignore || attr.AllAccess)
                {
                    continue;
                }
                if (!pg.Permissions.Any(x => x.Title == handlerTittle))
                {
                    pg.Permissions.Add(new PermissionDataViewModel
                    {
                        Name = handlerName,
                        PermissionGroupTitle = groupTitle,
                        Title = handlerTittle,
                    });
                }
            }
            logger.LogInformation("Call Permission Provider");
            return permissionProvider.Update(permitionGroupList);
        }
    }
}
