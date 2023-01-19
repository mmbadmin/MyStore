namespace MyStore.Application.Users.Commands.Login
{
    using CacheManager.Core;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using RExtension;
    using MyStore.Application.Commons;
    using MyStore.Common.Application.Attributes;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [AppAccess(nameof(User), "Users", "Login To MyStore", Ignore = true)]
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginViewModel>
    {
        private readonly IBaseRepository<User, Guid> userRepository;
        private readonly IBaseRepository<UserSession, int> userSessionRepository;
        private readonly IBaseRepository<Setting, int> settingRepository;
        private readonly ITokenProvider tokenProvider;
        private readonly ICurrentUserInfo currentUserInfo;

        public UserLoginCommandHandler(IBaseRepository<User, Guid> userRepository,
                                       IBaseRepository<UserSession, int> userSessionRepository,
                                       IBaseRepository<Setting, int> settingRepository,
                                       ITokenProvider tokenProvider,
                                       ICurrentUserInfo currentUserInfo)
        {
            this.userRepository = userRepository;
            this.userSessionRepository = userSessionRepository;
            this.settingRepository = settingRepository;
            this.tokenProvider = tokenProvider;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task<UserLoginViewModel> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var maxFailCount = await settingRepository.GetIntAsync(Texts.Setting.UserMaxFailAttemptCount, 20);
            var lockDuration = await settingRepository.GetIntAsync(Texts.Setting.UserLockDuration, 60);
            var tryDuration = await settingRepository.GetIntAsync(Texts.Setting.UserTryDuration, 50);
            var sessionDuration = await settingRepository.GetIntAsync(Texts.Setting.UserSessionDuration, 24);
            var automaticUnlock = await settingRepository.GetBooleanAsync(Texts.Setting.UserAutomaticUnlock, true);
            var singleSession = await settingRepository.GetBooleanAsync(Texts.Setting.UserSingleSession, false);

            var all = await userRepository.ListAsync();
            var loweredUserName = request.UserName.ToLower().FullTrim();
            var user = await userRepository.FindAsync(predicate: x => x.LoweredUserName == loweredUserName);
            if (user is null || !user.IsActive)
            {
                throw BaseException.BadRequest(Texts.Users.InvalidUserNameOrPassword);
            }
            if (user.IsLocked)
            {
                if (user.LastFailedAttempt?.AddMinutes(lockDuration) >= DateTime.Now)
                {
                    throw BaseException.BadRequest(Texts.Users.TooManyFailedLoginAttempt);
                }
                else if (automaticUnlock)
                {
                    user.Unlock();
                    await userRepository.UpdateAsync(user);
                }
                else
                {
                    throw BaseException.BadRequest(Texts.Users.TooManyFailedLoginAttempt);
                }
            }
            var checkPassword = PasswordHelper.Verify(request.Password, user.Password);
            if (!checkPassword)
            {
                if (user.LastFailedAttempt?.AddMinutes(tryDuration) <= DateTime.Now)
                {
                    user.Unlock();
                }
                user.IncreaseFailedAttempt();
                if (user.FailedAttemptCount >= maxFailCount)
                {
                    user.Lock(DateTime.Now);
                }
                await userRepository.UpdateAsync(user);
                throw BaseException.BadRequest(Texts.Users.InvalidUserNameOrPassword);
            }
            else
            {
                user.Unlock();
                await userRepository.UpdateAsync(user);
            }

            if (singleSession)
            {
                var sessions = await userSessionRepository.ListAsync(predicate: x => !x.IsExpired && x.UserId == user.Id);
                sessions.ForEach(x => { x.Expired(); });
                await userSessionRepository.UpdateRangeAsync(sessions);
            }
            var userSession = new UserSession(user.Id,
                                              Guid.NewGuid().ToString(),
                                              DateTime.Now.AddHours(sessionDuration),
                                              currentUserInfo.ConnectionInfo);
            await userSessionRepository.AddAsync(userSession);

            var tokenString = tokenProvider.CreateToken(user.Id, userSession.SessionKey);
            return new UserLoginViewModel
            {
                Token = tokenString,
            };
        }
    }
}
