namespace MyStore.Application.Roles.Queries.GetPagedList
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoleGetPagedListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description‌")]
        public string Description { get; set; }
    }
}
