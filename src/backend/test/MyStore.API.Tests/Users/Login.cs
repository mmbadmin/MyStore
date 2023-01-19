namespace MyStore.API.Tests.Users
{
    using Shouldly;
    using MyStore.API;
    using MyStore.API.Tests;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Login;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class Login : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public Login(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task EmptyUserNameTest()
        {
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                Password = "12345678",
                UserName = string.Empty,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task EmptyPasswordTest()
        {
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                Password = string.Empty,
                UserName = "LoginUserApiTestSeedUserName",
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WrongUserNameTest()
        {
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                Password = "12345678",
                UserName = "LoginUserApiTestSeedUser",
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WrongPasswordTest()
        {
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                Password = "12345687",
                UserName = "LoginUserApiTestSeedUserName",
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SuccessfulTest()
        {
            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
