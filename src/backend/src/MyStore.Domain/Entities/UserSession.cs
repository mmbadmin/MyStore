namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using System;

    public class UserSession : BaseEntity<int>, IAggregateRoot, IAuditableEntity
    {
        protected UserSession()
        {
        }

        public UserSession(Guid userId,
                           string sessionKey,
                           DateTime expireDate,
                           string connectionInfo)
        {
            UserId = userId;
            SessionKey = sessionKey;
            ExpireDate = expireDate;
            IsExpired = false;
            ConnectionInfo = connectionInfo;
        }

        public Guid UserId { get; private set; }

        public string SessionKey { get; private set; }

        public DateTime ExpireDate { get; private set; }

        public bool IsExpired { get; private set; }

        public string ConnectionInfo { get; private set; }

        public virtual User User { get; private set; }

        public void Expired()
        {
            IsExpired = true;
        }
    }
}
