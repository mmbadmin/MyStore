namespace MyStore.Application.Users.Commands.Create
{
    using FluentValidation;
    using RExtension;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Utilities;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        private readonly IBaseRepository<User, Guid> userRepository;

        public UserCreateCommandValidator(IBaseRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
            RuleFor(v => v.FirstName).NotEmpty()
                                    .MaximumLength(50);
            RuleFor(v => v.LastName).NotEmpty()
                                    .MaximumLength(50);
            RuleFor(v => v.UserName).NotEmpty()
                                    .Length(4, 50)
                                    .Matches("^(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")
                                    .WithMessage(Texts.Users.UserNameValidation)
                                    .MustAsync(IsUserNameUnique)
                                    .WithMessage(Texts.Users.IsUserNameUnique);
            RuleFor(v => v.Password).NotEmpty()
                                    .MinimumLength(8);
        }

        public async Task<bool> IsUserNameUnique(string userName, CancellationToken cancellationToken)
        {
            userName = userName.ToLower().FullTrim();
            return !(await userRepository.ListAsync(selector: x => x.LoweredUserName)).CheckDuplicate(userName);
        }
    }
}
