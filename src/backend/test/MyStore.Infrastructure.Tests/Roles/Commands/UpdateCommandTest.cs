namespace MyStore.Infrastructure.Tests.Roles.Commands
{
    using Shouldly;
    using MyStore.Application.Roles.Commands.Update;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class UpdateCommandTest
    {
        [Fact]
        public async Task RolePersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            dbContext.Roles.Add(initRole);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RoleUpdateCommand
            {
                Id = initRole.Id,
                Title = TestHelper.RandomString(10),
                Description = TestHelper.RandomString(10),
            };
            var handler = new RoleUpdateCommandHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            var role = dbContext.Roles.FirstOrDefault(x => x.Id == initRole.Id);
            role.ShouldNotBeNull();
            role.Title.ShouldBe(command.Title);
            role.Description.ShouldBe(command.Description);
            role.IsSystemAdmin.ShouldBeFalse();
        }
    }
}
