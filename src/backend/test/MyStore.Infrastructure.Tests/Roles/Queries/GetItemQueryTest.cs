namespace MyStore.Infrastructure.Tests.Roles.Queries
{
    using Shouldly;
    using MyStore.Application.Roles.Queries.GetItem;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class GetItemQueryTest
    {
        [Fact]
        public async Task RolePersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            dbContext.Roles.Add(initRole);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RoleGetItemQuery
            {
                Id = initRole.Id,
            };
            var handler = new RoleGetItemQueryHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldNotBeNull();
            result.Title.ShouldBe(initRole.Title);
            result.Description.ShouldBe(initRole.Description);
        }
    }
}
