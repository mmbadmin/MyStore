namespace MyStore.Infrastructure.Tests.Roles.Queries
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Application.Roles.Queries.GetList;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using Shouldly;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class GetListQueryTest
    {
        [Fact]
        public async Task RolePersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            var initRole1 = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            var initRole2 = new Role(Guid.NewGuid(), TestHelper.RandomString(10), TestHelper.RandomString(10));
            dbContext.Roles.Add(initRole1);
            dbContext.Roles.Add(initRole2);
            dbContext.SaveChanges();
            IBaseRepository<Role, Guid> roleRepository = new EFRepository<Role, Guid>(dbContext);
            var command = new RoleGetListQuery();
            var handler = new RoleGetListQueryHandler(roleRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(2);
        }
    }
}
