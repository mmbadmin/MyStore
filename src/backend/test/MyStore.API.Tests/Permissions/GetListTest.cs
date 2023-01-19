namespace MyStore.API.Tests.Permissions
{

    using Shouldly;
    using MyStore.Domain.Entities;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class GetListTest : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public GetListTest(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task SuccessTest()
        {
            var permission = new PermissionGroup
            {
                Title = "Title",
                Name = "Name",
                Permissions = new List<Permission>
                {
                    new Permission
                    {
                        Title = "Title",
                        Name = "Name",
                    },
                },
            };
            factory.CreateEntity(permission);
            var client = await factory.GetAdminClientAsync();
            var response = await client.GetAsync($"/api/v1/Permission");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
