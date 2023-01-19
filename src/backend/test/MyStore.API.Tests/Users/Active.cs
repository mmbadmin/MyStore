namespace MyStore.API.Tests.Users
{
    using Shouldly;
    using MyStore.API;
    using MyStore.API.Tests;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Active;
    using MyStore.Common.API.Models;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class Active : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public Active(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task EmptyUserIdTest()
        {
            var client = await factory.GetAdminClientAsync();

            var command = new UserActiveCommand
            {
                UserId = Guid.Empty,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/Deactive", content);
            var result = await response.GetResponseContent<MyStoreResponse>();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WrongUserIdTest()
        {
            var client = await factory.GetAdminClientAsync();
            var command = new UserActiveCommand
            {
                UserId = Guid.NewGuid(),
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/Deactive", content);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SuccessfulTest()
        {
            var userName = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(8);

            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(TestHelper.RandomString(10)));
            user.Active();
            factory.CreateEntity(user);
            var client = await factory.GetAdminClientAsync();
            var command = new UserActiveCommand
            {
                UserId = user.Id,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/Deactive", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultUser = factory.GetEntity<User>(x => x.Id == user.Id);
            resultUser.IsActive.ShouldBeFalse();
        }
    }
}
