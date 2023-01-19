namespace MyStore.Infrastructure.Tests.Roles.Commands
{
    using Shouldly;
    using MyStore.Application.Roles.Commands.Delete;
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

    public class DeleteCommandTest
    {
        [Fact]
        public async Task RolePersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            dbContext.Roles.Add(initRole);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RoleDeleteCommand
            {
                Id = initRole.Id,
            };
            var handler = new RoleDeleteCommandHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBeTrue();
            var role = dbContext.Roles.FirstOrDefault(x => x.Id == initRole.Id);
            role.ShouldBeNull();
        }
    }
}
