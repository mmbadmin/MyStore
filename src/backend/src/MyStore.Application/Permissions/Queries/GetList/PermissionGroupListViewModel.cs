namespace MyStore.Application.Permissions.Queries.GetList
{
    using System.Collections.Generic;

    public class PermissionGroupListViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<PermissionListViewModel> Permissions { get; set; }
    }
}
