namespace MyStore.Domain.Tests
{
    using Shouldly;
    using MyStore.Domain.Entities;
    using System;
    using Xunit;

    public class UserSessionTest
    {
        [Fact]
        public void CreateTest()
        {
            var userId = Guid.NewGuid();
            var expireDate = DateTime.Now;
            var userSession = new UserSession(userId, "SessionKey", expireDate, "Connection Info");

            userSession.ShouldNotBeNull();
            userSession.UserId.ShouldBe(userId);
            userSession.ExpireDate.ShouldBe(expireDate);
            userSession.ConnectionInfo.ShouldBe("Connection Info");
            userSession.SessionKey.ShouldBe("SessionKey");
            userSession.IsExpired.ShouldBeFalse();
        }

        [Fact]
        public void UpdateTest()
        {
            var userId = Guid.NewGuid();
            var expireDate = DateTime.Now;
            var userSession = new UserSession(userId, "SessionKey", expireDate, "Connection Info");

            userSession.ShouldNotBeNull();
            userSession.Expired();
            userSession.IsExpired.ShouldBeTrue();
            userSession.UserId.ShouldBe(userId);
            userSession.ExpireDate.ShouldBe(expireDate);
            userSession.ConnectionInfo.ShouldBe("Connection Info");
            userSession.SessionKey.ShouldBe("SessionKey");
        }
    }
}
