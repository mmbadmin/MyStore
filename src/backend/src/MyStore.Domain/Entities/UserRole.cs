namespace MyStore.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserRole
    {
        protected UserRole()
        {
        }

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        [Display(Name = "User")]
        public Guid UserId { get; set; }

        [Display(Name = "Role")]
        public Guid RoleId { get; set; }
    }
}
