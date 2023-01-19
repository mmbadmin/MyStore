namespace MyStore.Common.Application.Models
{
    using System.Collections.Generic;

    public class PermissionGroupDataViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public IList<PermissionDataViewModel> Permissions { get; set; } = new List<PermissionDataViewModel>();
    }
}
