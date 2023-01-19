namespace MyStore.Application.Users.Commands.Update
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using FluentValidation;
    using System;

    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator(IBaseRepository<User, Guid> userRepository)
        {
            RuleFor(v => v.FirstName).NotEmpty()
                                    .MaximumLength(50);
            RuleFor(v => v.LastName).NotEmpty()
                                    .MaximumLength(50);
        }
    }
}
