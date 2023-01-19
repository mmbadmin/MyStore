namespace MyStore.Application.Users.Commands.ChangePassword
{
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserChangePasswordCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Display(Name = "Current PassWord")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
