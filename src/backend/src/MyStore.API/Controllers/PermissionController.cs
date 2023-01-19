namespace MyStore.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyStore.Application.Permissions.Queries.GetAllPermission;
    using MyStore.Application.Permissions.Queries.GetAllPermissionTitle;
    using MyStore.Application.Permissions.Queries.GetList;
    using MyStore.Common.API.Mvc;
    using System.Threading.Tasks;

    public class PermissionController : BaseController
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new PermissionGetListQuery());
            return Ok(result);
        }

        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new PermissionGetAllQuery());
            return Ok(result);
        }

        [HttpGet("AllTitle")]
        public async Task<IActionResult> GetAllTitle()
        {
            var result = await Mediator.Send(new PermissionGetAllTitleQuery());
            return Ok(result);
        }
    }
}
