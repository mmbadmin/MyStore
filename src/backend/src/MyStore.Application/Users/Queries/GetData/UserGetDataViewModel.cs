namespace MyStore.Application.Users.Queries.GetData
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserGetDataViewModel
    {
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "")]
        public IEnumerable<string> Permissions { get; set; }
    }
}
