namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PermissionGroup : BaseEntity<int>, IAggregateRoot, IAuditableEntity, IDeletedEntity
    {
        public PermissionGroup()
        {
            Permissions = new HashSet<Permission>();
        }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
