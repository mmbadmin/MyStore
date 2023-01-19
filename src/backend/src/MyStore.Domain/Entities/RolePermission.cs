namespace MyStore.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RolePermission
    {
        protected RolePermission()
        {
        }

        public RolePermission(Guid roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        [Display(Name = "Role")]
        public Guid RoleId { get; private set; }

        [Display(Name = "Permission")]
        public int PermissionId { get; private set; }
    }
}
