namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Permission : BaseEntity<int>, IAggregateRoot, IAuditableEntity, IDeletedEntity
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        [Display(Name = " Permission Group")]
        public int PermissionGroupId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual PermissionGroup PermissionGroup { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
