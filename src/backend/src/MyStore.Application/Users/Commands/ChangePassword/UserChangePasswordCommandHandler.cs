namespace MyStore.Application.Users.Commands.ChangePassword
{
    using MediatR;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Change Password", AllAccess = true)]
    public class UserChangePasswordCommandHandler : IRequestHandler<UserChangePasswordCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;
        private readonly IBaseRepository<UserSession, int> sessionRepository;

        public UserChangePasswordCommandHandler(IBaseRepository<User, Guid> userRepository,
                                                IBaseRepository<UserSession, int> sessionRepository)
        {
            this.userRepository = userRepository;
            this.sessionRepository = sessionRepository;
        }

        public async Task<bool> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.Id);
            if (user is null)
            {
                throw BaseException.BadRequest();
            }
            var checkPassword = PasswordHelper.Verify(request.OldPassword, user.Password);
            if (!checkPassword)
            {
                throw BaseException.BadRequest(Texts.Users.InvalidPassword);
            }
            var userSession = await sessionRepository.FindAsync(predicate: x => x.UserId == user.Id &&
                                                                           !x.IsExpired &&
                                                                           x.ExpireDate.Date >= DateTime.Now.Date);
            if (!(userSession is null))
            {
                userSession.Expired();
                await sessionRepository.UpdateAsync(userSession);
            }
            user.UpdatePassword(PasswordHelper.Hash(request.NewPassword));
            return await userRepository.UpdateAsync(user);
        }
    }
}
