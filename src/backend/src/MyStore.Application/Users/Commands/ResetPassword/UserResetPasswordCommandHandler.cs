namespace MyStore.Application.Users.Commands.ResetPassword
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

    [AppAccess(nameof(User), "User", "Change PassWord")]
    public class UserResetPasswordCommandHandler : IRequestHandler<UserResetPasswordCommand, bool>
    {
        private readonly IBaseRepository<User, Guid> userRepository;
        private readonly IBaseRepository<UserSession, int> sessionRepository;

        public UserResetPasswordCommandHandler(IBaseRepository<User, Guid> userRepository,
                                               IBaseRepository<UserSession, int> sessionRepository)
        {
            this.userRepository = userRepository;
            this.sessionRepository = sessionRepository;
        }

        public async Task<bool> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(predicate: x => x.Id == request.Id);
            if (user is null)
            {
                throw BaseException.BadRequest();
            }
            var userSessions = await sessionRepository.ListAsync(predicate: x => x.UserId == user.Id &&
                                                                            x.ExpireDate.Date >= DateTime.Now.Date &&
                                                                            !x.IsExpired);
            foreach (var userSession in userSessions)
            {
                userSession.Expired();
            }
            await sessionRepository.UpdateRangeAsync(userSessions);
            user.ResetPassword(PasswordHelper.Hash(request.Password));
            return await userRepository.UpdateAsync(user);
        }
    }
}
