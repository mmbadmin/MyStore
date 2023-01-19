namespace MyStore.IntegrationTest.Users
{
    using Shouldly;
    using MyStore.API;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.ResetPassword;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.IntegrationTest;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class UserAuthorizationAdminChengePasswordTest : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public UserAuthorizationAdminChengePasswordTest(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Exec()
        {
            var username = TestHelper.RandomString(10);
            var oldPassword = TestHelper.RandomString(8);
            var newPassword = TestHelper.RandomString(8);
            var user = CreateUser(username, oldPassword);
            var userClient = await factory.GetNonAdminClientAsync(username, oldPassword);
            await GetUserDataSuccess(userClient);
            await ResetUserPassword(user.Id, newPassword);
            await GetUserDataFailed(userClient);
            userClient = await factory.GetNonAdminClientAsync(username, newPassword);
            await GetUserDataSuccess(userClient);
        }

        private User CreateUser(string username, string password)
        {
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
            return user;
        }

        private async Task GetUserDataSuccess(HttpClient client)
        {
            var response = await client.GetAsync($"/api/v1/User/Data");
            response.EnsureSuccessStatusCode();
        }

        private async Task GetUserDataFailed(HttpClient client)
        {
            var response = await client.GetAsync($"/api/v1/User/Data");
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        private async Task ResetUserPassword(Guid userId, string password)
        {
            var client = await factory.GetAdminClientAsync();
            var command = new UserResetPasswordCommand
            {
                Password = password,
                ConfirmPassword = password,
                Id = userId,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ResetPassword", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
