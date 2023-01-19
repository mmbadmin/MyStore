namespace MyStore.Application.Users.Queries.GetPagedList
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserGetPagedListViewModel
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Is Active ?")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Locked?")]
        public bool IsLocked { get; set; }
    }
}
