namespace MyStore.Infrastructure.Tests.Users.Commands
{
    using Shouldly;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Create;
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

    public class CreateCommandTest : BaseTest
    {
        [Fact]
        public async Task UserPersistenceDataTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = "Persistence Data Test FirstName Create Command",
                LastName = "Persistence Data Test LastName Create Command",
                Password = "Persistence Data Test Password Create Command",
                UserName = "Persistence Data Test UserName Create Command",
            };
            var handler = new UserCreateCommandHandler(userRepository);
            var result = await handler.Handle(command, CancellationToken.None);
            var user = dbContext.Users.FirstOrDefault();
            result.ShouldBe(true);
            user.UserName.ShouldBe(command.UserName);
        }

        [Fact]
        public void UserCreateCommandValidInputTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = TestHelper.RandomString(10),
                LastName = TestHelper.RandomString(10),
                UserName = TestHelper.RandomString(10),
                Password = TestHelper.RandomString(8),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(true);
            var newUser = dbContext.Users.FirstOrDefault(x => x.LoweredUserName == command.UserName.ToLower());
        }

        [Fact]
        public void UserCreateCommandInValidInputUserNameWithSpaceTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = TestHelper.RandomString(10),
                LastName = TestHelper.RandomString(10),
                UserName = TestHelper.RandomString(4) + " " + TestHelper.RandomString(4),
                Password = TestHelper.RandomString(8),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe(Texts.Users.UserNameValidation);
            result.Errors[0].PropertyName.ShouldBe("UserName");
        }

        [Fact]
        public void UserCreateCommandInValidFirstNameInputTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = string.Empty,
                LastName = TestHelper.RandomString(10),
                Password = TestHelper.RandomString(8),
                UserName = TestHelper.RandomString(10),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe("It is necessary to enter name.");
            result.Errors[0].PropertyName.ShouldBe("FirstName");
        }

        [Fact]
        public void UserCreateCommandInValidLastNameInputTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = TestHelper.RandomString(10),
                LastName = string.Empty,
                Password = TestHelper.RandomString(8),
                UserName = TestHelper.RandomString(10),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe("Enter last name is required.");
            result.Errors[0].PropertyName.ShouldBe("LastName");
        }

        [Fact]
        public void UserCreateCommandDuplicateUserNameTest()
        {
            var dbContext = DbInit.CreateDb();
            var user = new User(Guid.NewGuid(),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                TestHelper.RandomString(10),
                                PasswordHelper.Hash(TestHelper.RandomString(10)));
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                UserName = user.UserName,
                FirstName = TestHelper.RandomString(10),
                LastName = TestHelper.RandomString(10),
                Password = TestHelper.RandomString(8),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe(Texts.Users.IsUserNameUnique);
            result.Errors[0].PropertyName.ShouldBe("UserName");
        }

        [Fact]
        public void UserCreateCommandValidUserNameLengthTest()
        {
            var dbContext = DbInit.CreateDb();
            IBaseRepository<User, Guid> userRepository = new EFRepository<User, Guid>(dbContext);
            var command = new UserCreateCommand
            {
                FirstName = TestHelper.RandomString(10),
                LastName = TestHelper.RandomString(10),
                Password = TestHelper.RandomString(8),
                UserName = TestHelper.RandomString(3),
            };
            var validator = new UserCreateCommandValidator(userRepository);
            var result = validator.Validate(command);
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe("'Username must be at least 6 and at most 50 characters long. But the entered value has 3 characters.");
            result.Errors[0].PropertyName.ShouldBe("UserName");
        }
    }
}
