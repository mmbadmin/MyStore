namespace MyStore.Infrastructure.Tests.Users.Commands
{
    using Moq;
    using Shouldly;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Login;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Domain.Enums;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class LoginCommandTest : BaseTest
    {
        [Fact]
        public async Task LoginSuccessfulTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);

            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password,
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            result.Token.ShouldNotBe(string.Empty);
        }

        [Fact]
        public void LoginInvalidUserNameTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = "12345678",
                UserName = "SuccessfulLoginTestUserName",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void LoginNullUserNameTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = "12345678",
                UserName = string.Empty,
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void LoginInvalidPasswordTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = "12345689",
                UserName = "SuccessfulLoginTestUserNameLoginUserCommand",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void LoginNullPasswordTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = string.Empty,
                UserName = "SuccessfulLoginTestUserNameLoginUserCommand",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void LoginUnmatchedUserNamePasswordTest()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = "123456798",
                UserName = "SuccessfulLoginTestUserNameLoginUserCommand",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void DiactiveUserLogin()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                Password = "123456798",
                UserName = "diactiveuserlogintest",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe("Your username or password is invalid");
        }

        [Fact]
        public void AccountLock()
        {
            const int failAttempt = 3;

            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            dbContext.Settings.Add(new Setting(Texts.Setting.UserMaxFailAttemptCount,
                                               Texts.Setting.UserMaxFailAttemptCount,
                                               failAttempt.ToString(),
                                               SettingValueType.Int));
            dbContext.SaveChanges();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password + "1",
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);
            for (var i = 0; i < failAttempt; i++)
            {
                Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                            .Message.ShouldBe(Texts.Users.InvalidUserNameOrPassword);
            }
            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe(Texts.Users.TooManyFailedLoginAttempt);
        }

        [Fact]
        public void AccountLockRejectLogin()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            user.Lock(DateTime.Now.AddMinutes(-30));
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password,
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);

            Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None))
                        .Message.ShouldBe(Texts.Users.TooManyFailedLoginAttempt);
        }

        [Fact]
        public async Task AccountUnLock()
        {
            var currentUserInfo = new Mock<ICurrentUserInfo>();
            var tokenProvider = new Mock<ITokenProvider>();

            tokenProvider.Setup(m => m.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns("Token");

            var dbContext = DbInit.CreateDb();

            var username = TestHelper.RandomString(10);
            var password = TestHelper.RandomString(10);
            var user = new User(Guid.NewGuid(),
                                username,
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(password));
            user.Active();
            user.Lock(DateTime.Now.AddHours(-2));
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            IBaseRepository<UserSession, int> userSessionRepository = new EFRepository<UserSession, int>(dbContext);
            IBaseRepository<Setting, int> settingRepository = new EFRepository<Setting, int>(dbContext);
            var command = new UserLoginCommand
            {
                UserName = username,
                Password = password,
            };
            var handler = new UserLoginCommandHandler(userRepository,
                                                      userSessionRepository,
                                                      settingRepository,
                                                      tokenProvider.Object,
                                                      currentUserInfo.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            result.Token.ShouldNotBe(string.Empty);
        }
    }
}
