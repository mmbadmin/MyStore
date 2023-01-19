namespace MyStore.IntegrationTest.Users
{
    using MyStore.API;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.ChangePassword;
    using MyStore.Application.Users.Commands.Login;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.IntegrationTest;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ChangePasswordTest : IClassFixture<MyStoreWebApplicationFactory<Startup>>
    {
        private readonly MyStoreWebApplicationFactory<Startup> factory;

        public ChangePasswordTest(MyStoreWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Exec()
        {
            var username = TestHelper.RandomString(10);
            var oldPassword = TestHelper.RandomString(8);
            var newPassword = TestHelper.RandomString(8);
            CreateUser(username, oldPassword);
            await ChangePassword(username, oldPassword, newPassword);
            await LoginWithNewPassword(username, newPassword);
        }

        private void CreateUser(string username, string password)
        {
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            factory.CreateEntity(user);
        }

        private async Task ChangePassword(string username, string oldPassword, string newPassword)
        {
            var client = await factory.GetNonAdminClientAsync(username, oldPassword);
            var command = new UserChangePasswordCommand
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = newPassword,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PutAsync($"/api/v1/User/ChangePassword", content);
            response.EnsureSuccessStatusCode();
        }

        private async Task LoginWithNewPassword(string username, string password)
        {
            var client = factory.GetAnonymousClient();
            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password,
            };
            var content = TestHelper.GetRequestContent(command);
            var response = await client.PostAsync($"/api/v1/User/Login", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
