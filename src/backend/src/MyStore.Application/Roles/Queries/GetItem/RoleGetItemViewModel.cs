namespace MyStore.Application.Roles.Queries.GetItem
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoleGetItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description‌")]
        public string Description { get; set; }
    }
}
