namespace MyStore.Application.Users.Queries.GetItem
{
    using System.ComponentModel.DataAnnotations;

    public class UserGetItemViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
