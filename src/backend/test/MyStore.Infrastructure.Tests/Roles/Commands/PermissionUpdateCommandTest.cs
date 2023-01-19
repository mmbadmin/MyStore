namespace MyStore.Infrastructure.Tests.Roles.Commands
{
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using MyStore.Application.Roles.Commands.PermissionUpdate;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class PermissionUpdateCommandTest
    {
        [Fact]
        public async Task RolePermissionPersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            var permissionGroup = new PermissionGroup
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            var permission1 = new Permission
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            var permission2 = new Permission
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            permissionGroup.Permissions.Add(permission1);
            permissionGroup.Permissions.Add(permission2);

            dbContext.Roles.Add(initRole);
            dbContext.PermissionGroups.Add(permissionGroup);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RolePermissionUpdateCommand
            {
                RoleId = initRole.Id,
                PermissionIds = new List<int> { permission1.Id, permission2.Id },
            };

            var handler = new RolePermissionUpdateCommandHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            var role = dbContext.Roles.Include(x => x.RolePermissions).FirstOrDefault(x => x.Id == initRole.Id);
            role.ShouldNotBeNull();
            role.Title.ShouldBe(initRole.Title);
            role.Description.ShouldBe(initRole.Description);
            role.IsSystemAdmin.ShouldBeFalse();
            role.RolePermissions.Count.ShouldBe(2);
        }

        [Fact]
        public async Task RolePermissionUpdateTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            var permissionGroup = new PermissionGroup
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            var permission1 = new Permission
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            var permission2 = new Permission
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            var permission3 = new Permission
            {
                Name = TestHelper.RandomString(10),
                Title = TestHelper.RandomString(10),
            };

            permissionGroup.Permissions.Add(permission1);
            permissionGroup.Permissions.Add(permission2);
            permissionGroup.Permissions.Add(permission3);

            dbContext.PermissionGroups.Add(permissionGroup);
            dbContext.SaveChanges();
            dbContext.Roles.Add(initRole);
            dbContext.SaveChanges();
            initRole.AddPermission(permission3.Id);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RolePermissionUpdateCommand
            {
                RoleId = initRole.Id,
                PermissionIds = new List<int> { permission1.Id, permission2.Id },
            };

            var handler = new RolePermissionUpdateCommandHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            var role = dbContext.Roles.Include(x => x.RolePermissions).FirstOrDefault(x => x.Id == initRole.Id);
            role.ShouldNotBeNull();
            role.Title.ShouldBe(initRole.Title);
            role.Description.ShouldBe(initRole.Description);
            role.IsSystemAdmin.ShouldBeFalse();
            role.RolePermissions.Count.ShouldBe(2);
            role.RolePermissions.Select(x => x.PermissionId).ShouldNotContain(permission3.Id);
        }
    }
}
