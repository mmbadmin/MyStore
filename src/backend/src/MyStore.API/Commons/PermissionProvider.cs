namespace MyStore.API.Commons
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PermissionProvider : IPermissionProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<PermissionProvider> logger;

        public PermissionProvider(IServiceProvider serviceProvider,
                                  ILogger<PermissionProvider> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task Update(IList<PermissionGroupDataViewModel> data)
        {
            logger.LogInformation("Permission Provider Start");
            using var scope = serviceProvider.CreateScope();
            var permissionGroupRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<PermissionGroup, int>>();
            var permissionRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Permission, int>>();
            var permissionGroups = await permissionGroupRepository.ListAsync();
            foreach (var permitionGroupItem in data)
            {
                var item = permissionGroups.FirstOrDefault(x => x.Title == permitionGroupItem.Title);
                if (item == null)
                {
                    var permitionGroup = new PermissionGroup
                    {
                        Title = permitionGroupItem.Title,
                        Name = permitionGroupItem.Name,
                    };
                    await permissionGroupRepository.AddAsync(permitionGroup);
                }
                else if (item.Name != permitionGroupItem.Name)
                {
                    item.Name = permitionGroupItem.Name;
                    await permissionGroupRepository.UpdateAsync(item);
                }
            }
            foreach (var permissionGroupItem in data)
            {
                var permissionGroupId = await permissionGroupRepository.FindAsync(predicate: x => x.Title == permissionGroupItem.Title,
                                                                                  selector: x => x.Id);

                var dbPermissions = permissionRepository.List(predicate: x => x.PermissionGroupId == permissionGroupId);
                var titles = permissionGroupItem.Permissions.Select(z => z.Title).ToList();
                var removedPermission = dbPermissions.Where(x => !titles.Contains(x.Title)).ToList();
                if (removedPermission.Count > 0)
                {
                    permissionRepository.RemoveRange(removedPermission);
                }

                foreach (var permissionItem in permissionGroupItem.Permissions)
                {
                    var item = await permissionRepository.FindAsync(predicate: x => x.Title == permissionItem.Title &&
                                                                                    x.PermissionGroupId == permissionGroupId);
                    if (item == null)
                    {
                        var permition = new Permission
                        {
                            Title = permissionItem.Title,
                            PermissionGroupId = permissionGroupId,
                            Name = permissionItem.Name,
                        };
                        await permissionRepository.AddAsync(permition);
                    }
                    else if (item.Name != permissionItem.Name)
                    {
                        item.Name = permissionItem.Name;
                        await permissionRepository.UpdateAsync(item);
                    }
                }
            }
            var permissionLessPermissionGroup = await permissionGroupRepository.ListAsync(predicate: x => x.Permissions.Count == 0);
            if (permissionLessPermissionGroup.Count > 0)
            {
                permissionGroupRepository.RemoveRange(permissionLessPermissionGroup);
            }
            logger.LogInformation("Permission Provider End");
        }
    }
}
