namespace MyStore.Application.Roles.Commands.Create
{
    using MediatR;
    using System.ComponentModel.DataAnnotations;

    public class RoleCreateCommand : IRequest<bool>
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description‌")]
        public string Description { get; set; }
    }
}
