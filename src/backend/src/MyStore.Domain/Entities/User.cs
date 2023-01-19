namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using RExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class User : BaseEntity<Guid>, IAggregateRoot, IAuditableEntity, IDeletedEntity
    {
        private readonly List<UserRole> userRoles = new List<UserRole>();

        protected User()
        {
            userRoles = new List<UserRole>();
        }

        public User(Guid id, string username, string firstname, string lastname, string password)
            : this()
        {
            Id = id;
            UserName = username.FullTrim();
            LoweredUserName = username.ToLower().FullTrim();
            FirstName = firstname;
            LastName = lastname;
            Password = password;
            IsPasswordSecure = false;
        }

        [Display(Name = "UserName")]
        public string UserName { get; private set; }

        [Display(Name = "Username in lower case")]
        public string LoweredUserName { get; private set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; private set; }

        [Display(Name = "LastName")]
        public string LastName { get; private set; }

        [Display(Name = "Password")]
        public string Password { get; private set; }

        [Display(Name = "Is Password Secure?")]
        public bool IsPasswordSecure { get; private set; }

        [Display(Name = "Is Active ?")]
        public bool IsActive { get; private set; }

        [Display(Name = "Failed Attempt Count")]
        public int FailedAttemptCount { get; private set; }

        [Display(Name = "Last Failed Attempt")]
        public DateTime? LastFailedAttempt { get; private set; }

        [Display(Name = "Is Locked?")]
        public bool IsLocked { get; private set; }

        [Display(Name = "Last Login Time")]
        public DateTime? LastLoginTime { get; private set; }

        [Display(Name = "User roles")]
        public IReadOnlyList<UserRole> UserRoles => userRoles.ToList();

        public void UpdateUserName(string username)
        {
            UserName = username.FullTrim();
            LoweredUserName = username.ToLower().FullTrim();
        }

        public void UpdateFirstName(string firstname)
        {
            FirstName = firstname;
        }

        public void UpdateLastName(string lastname)
        {
            LastName = lastname;
        }

        public void ResetPassword(string password)
        {
            Password = password;
            IsPasswordSecure = false;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
            IsPasswordSecure = true;
        }

        public void Active()
        {
            IsActive = true;
        }

        public void Deactive()
        {
            IsActive = false;
        }

        public void Lock(DateTime dateTime)
        {
            IsLocked = true;
            LastFailedAttempt = dateTime;
        }

        public void Unlock()
        {
            IsLocked = false;
            LastFailedAttempt = null;
            FailedAttemptCount = 0;
        }

        public void IncreaseFailedAttempt()
        {
            LastFailedAttempt = DateTime.Now;
            FailedAttemptCount++;
        }

        public void ClearRoles()
        {
            userRoles.Clear();
        }

        public void AddRole(Guid roleId)
        {
            if (userRoles.Any(x => x.RoleId == roleId))
            {
                return;
            }
            userRoles.Add(new UserRole(Id, roleId));
        }
    }
}
