namespace MyStore.Common.Application.Interfaces
{
    using MyStore.Common.Application.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPermissionProvider
    {
        Task Update(IList<PermissionGroupDataViewModel> data);
    }
}
