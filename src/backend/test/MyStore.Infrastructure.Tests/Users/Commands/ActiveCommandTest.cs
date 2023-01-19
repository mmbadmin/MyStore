namespace MyStore.Infrastructure.Tests.Users.Commands
{
    using Shouldly;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Active;
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

    public class ActiveCommandTest : BaseTest
    {
        [Fact]
        public async Task SuccessfulActivationTest()
        {
            var dbContext = DbInit.CreateDb();
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(TestHelper.RandomString(10)));
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            var command = new UserActiveCommand
            {
                UserId = user.Id,
            };
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var handler = new UserActiveCommandHandler(userRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            result.ShouldBe(true);
            var item = dbContext.Users.FirstOrDefault();
            item.IsActive.ShouldBe(true);
        }

        [Fact]
        public void InvalidUserText()
        {
            var dbContext = DbInit.CreateDb();

            var command = new UserActiveCommand
            {
                UserId = Guid.NewGuid(),
            };
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var handler = new UserActiveCommandHandler(userRepository);
            var excpetion = Should.Throw<BaseException>(async () => await handler.Handle(command, CancellationToken.None));
            excpetion.Status.ShouldBe(404);
        }

        [Fact]
        public void ValidationText()
        {
            var command = new UserActiveCommand
            {
                UserId = Guid.Empty,
            };
            var validator = new UserActiveCommandValidator();
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
        }
    }
}
