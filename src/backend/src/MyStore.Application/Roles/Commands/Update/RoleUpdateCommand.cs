namespace MyStore.Application.Roles.Commands.Update
{
    using MediatR;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RoleUpdateCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description‌")]
        public string Description { get; set; }
    }
}
