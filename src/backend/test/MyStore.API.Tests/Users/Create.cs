namespace MyStore.API.Tests.Users
{
    using Shouldly;
    using MyStore.API;
    using MyStore.API.Tests;
    using MyStore.Application.Users.Commands.Create;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class Create : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public Create(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task SuccessfulCommandTest()
        {
            var client = await factory.GetAdminClientAsync();

            var command = new UserCreateCommand
            {
                FirstName = "API Create Valid Command Test FirsName",
                LastName = "API Create Valid Command Test LastName",
                Password = "12345678",
                UserName = "APICreateValidCommandTestUserName",
            };

            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var user = factory.GetEntity<User>(x => x.LoweredUserName == command.UserName.ToLower());
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task InvalidPasswordTest()
        {
            var client = await factory.GetAdminClientAsync();

            var command = new UserCreateCommand
            {
                FirstName = "API Create Command Test FirsName",
                LastName = "API Create Command Test LastName",
                Password = "1234567",
                UserName = "APICreateCommandTestFirsName",
            };

            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
