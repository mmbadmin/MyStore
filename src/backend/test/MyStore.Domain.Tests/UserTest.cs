namespace MyStore.Domain.Tests
{
    using Shouldly;
    using MyStore.Domain.Entities;
    using System;
    using Xunit;

    public class UserTest
    {
        [Fact]
        public void CreateTest()
        {
            var id = Guid.NewGuid();
            var user = new User(id, "UserName", "FirstName", "LastName", "Password");

            user.Id.ShouldBe(id);
            user.UserName.ShouldBe("UserName");
            user.LoweredUserName.ShouldBe("username");
            user.FirstName.ShouldBe("FirstName");
            user.LastName.ShouldBe("LastName");
            user.Password.ShouldBe("Password");
            user.IsActive.ShouldBeFalse();
            user.IsLocked.ShouldBeFalse();
            user.IsPasswordSecure.ShouldBeFalse();
            user.LastFailedAttempt.ShouldBeNull();
            user.LastLoginTime.ShouldBeNull();
            user.FailedAttemptCount.ShouldBe(0);
            user.UserRoles.ShouldNotBeNull();
            user.UserRoles.Count.ShouldBe(0);
        }
    }
}
