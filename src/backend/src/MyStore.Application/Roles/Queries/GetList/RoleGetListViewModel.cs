namespace MyStore.Application.Roles.Queries.GetList
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoleGetListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }
    }
}
