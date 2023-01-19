namespace MyStore.Infrastructure.Tests.Users.Commands
{
    using Shouldly;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Deactive;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Test;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using MyStore.Infrastructure.Tests.Helper;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class DeactiveCommandTest : BaseTest
    {
        [Fact]
        public async Task SuccessfulDeactivateTest()
        {
            var dbContext = DbInit.CreateDb();
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(TestHelper.RandomString(10)));
            user.Active();
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            var command = new UserDeactiveCommand
            {
                UserId = user.Id,
            };
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var handler = new UserDeactiveCommandHandler(userRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBe(true);
            var item = dbContext.Users.FirstOrDefault();
            item.IsActive.ShouldBe(false);
        }

        [Fact]
        public void InValidUserText()
        {
            var dbContext = DbInit.CreateDb();

            var command = new UserDeactiveCommand
            {
                UserId = Guid.NewGuid(),
            };
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var handler = new UserDeactiveCommandHandler(userRepository);
            var excpetion = Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None));
            excpetion.Status.ShouldBe(404);
        }

        [Fact]
        public void ValidationText()
        {
            var command = new UserDeactiveCommand
            {
                UserId = Guid.Empty,
            };
            var validator = new UserDeactiveCommandValidator();
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
        }
    }
}
