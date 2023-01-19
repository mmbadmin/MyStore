namespace MyStore.API.Tests.Users
{
    using Shouldly;
    using MyStore.API;
    using MyStore.API.Tests;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.ChangePassword;
    using MyStore.Common.API.Models;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class ChangePassword : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public ChangePassword(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task EmptyOldPasswordTest()
        {
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
            var client = await factory.GetNonAdminClientAsync(user.UserName, password);
            var command = new UserChangePasswordCommand
            {
                ConfirmPassword = "12345678",
                NewPassword = "12345678",
                OldPassword = string.Empty,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ChangePassword", content);
            var result = await response.GetResponseContent<MyStoreResponse>();
            result.Message.Count().ShouldBe(1);
            result.Message.ShouldContain(Texts.Users.OldPasswordIsRequired);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WrongOldPasswordTest()
        {
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
            var client = await factory.GetNonAdminClientAsync(user.UserName, password);
            var command = new UserChangePasswordCommand
            {
                ConfirmPassword = "12345678",
                NewPassword = "12345678",
                OldPassword = "87654321",
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ChangePassword", content);
            var result = await response.GetResponseContent<MyStoreResponse>();
            result.Message.Count().ShouldBe(1);
            result.Message.ShouldContain(Texts.Users.InvalidPassword);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WrongConfirmPasswordTest()
        {
            var client = await factory.GetAdminClientAsync();
            var command = new UserChangePasswordCommand
            {
                ConfirmPassword = "12345678",
                NewPassword = "87654321",
                OldPassword = "12345678",
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ChangePassword", content);
            var result = await response.GetResponseContent<MyStoreResponse>();
            result.Message.Count().ShouldBe(1);
            result.Message.ShouldContain(Texts.Users.NewPasswordAndConfirmPasswordInconsistent);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SuccessfulChangePasswordTest()
        {
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
            var client = await factory.GetNonAdminClientAsync(user.UserName, password);
            var command = new UserChangePasswordCommand
            {
                ConfirmPassword = "87654321",
                NewPassword = "87654321",
                OldPassword = password,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ChangePassword", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var newUser = factory.GetEntity<User>(x => x.Id == user.Id);
            newUser.ShouldNotBeNull();
            newUser.IsPasswordSecure.ShouldBeTrue();
            PasswordHelper.Verify("87654321", newUser.Password).ShouldBeTrue();
        }
    }
}
