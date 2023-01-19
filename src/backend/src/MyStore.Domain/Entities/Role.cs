namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Role : BaseEntity<Guid>, IAggregateRoot, IAuditableEntity, IDeletedEntity
    {
        private readonly List<UserRole> userRoles = new List<UserRole>();

        private readonly List<RolePermission> rolePermissions = new List<RolePermission>();

        protected Role()
        {
            userRoles = new List<UserRole>();
            rolePermissions = new List<RolePermission>();
        }

        public Role(Guid id, string title, string description)
            : this()
        {
            Id = id;
            Title = title;
            Description = description;
            IsSystemAdmin = false;
        }

        [Display(Name = "Title")]
        public string Title { get; private set; }

        [Display(Name = "Description‌")]
        public string Description { get; private set; }

        public bool IsSystemAdmin { get; private set; }

        public IReadOnlyList<UserRole> UserRoles => userRoles.ToList();

        public IReadOnlyList<RolePermission> RolePermissions => rolePermissions.ToList();

        public void UpdateIsSystemAdmin(bool isSystemAdmin)
        {
            IsSystemAdmin = isSystemAdmin;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void ClearPermissions()
        {
            rolePermissions.Clear();
        }

        public void AddPermission(int permissionId)
        {
            if (rolePermissions.Any(x => x.PermissionId == permissionId))
            {
                return;
            }
            rolePermissions.Add(new RolePermission(Id, permissionId));
        }
    }
}
