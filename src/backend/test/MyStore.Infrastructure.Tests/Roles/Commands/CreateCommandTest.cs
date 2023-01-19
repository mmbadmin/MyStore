namespace MyStore.Infrastructure.Tests.Roles.Commands
{
    using Shouldly;
    using MyStore.Application.Roles.Commands.Create;
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

    public class CreateCommandTest
    {
        [Fact]
        public async Task RolePersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RoleCreateCommand
            {
                Title = TestHelper.RandomString(10),
                Description = TestHelper.RandomString(10),
            };
            var handler = new RoleCreateCommandHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            var role = dbContext.Roles.FirstOrDefault();
            role.Title.ShouldBe(command.Title);
            role.Description.ShouldBe(command.Description);
            role.IsSystemAdmin.ShouldBeFalse();
        }
    }
}
