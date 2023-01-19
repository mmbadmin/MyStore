namespace MyStore.API.Tests.Authorization
{
    using Shouldly;
    using MyStore.API;
    using MyStore.API.Tests;
    using MyStore.Application.Users.Commands.Create;
    using MyStore.Common.Test;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class Authorization : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public Authorization(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task AnonymousRequest()
        {
            var client = factory.GetAnonymousClient();

            var response = await client.GetAsync($"/api/v1/User/Data");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ForbiddenRequest()
        {
            var client = await factory.GetNonAdminClientAsync();

            var command = new UserCreateCommand
            {
                FirstName = TestHelper.RandomString(10),
                LastName = TestHelper.RandomString(10),
                Password = TestHelper.RandomString(10),
                UserName = TestHelper.RandomString(10),
            };

            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User", content);

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AuthorizedRequest()
        {
            var client = await factory.GetAdminClientAsync();

            var response = await client.GetAsync($"/api/v1/User/Data");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
