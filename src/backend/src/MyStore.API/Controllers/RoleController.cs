namespace MyStore.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyStore.Application.Roles.Commands.Create;
    using MyStore.Application.Roles.Commands.Delete;
    using MyStore.Application.Roles.Commands.PermissionUpdate;
    using MyStore.Application.Roles.Commands.Update;
    using MyStore.Application.Roles.Queries.GetItem;
    using MyStore.Application.Roles.Queries.GetList;
    using MyStore.Application.Roles.Queries.GetPagedList;
    using MyStore.Application.Roles.Queries.GetPermissions;
    using MyStore.Common.API.Models;
    using MyStore.Common.API.Mvc;
    using MyStore.Common.Application.Interfaces;
    using System;
    using System.Threading.Tasks;

    public class RoleController : BaseController
    {
        [HttpGet("Filter")]
        [Authorize]
        [ProducesResponseType(typeof(IPagedList<RoleGetPagedListViewModel>), 200)]
        public async Task<IActionResult> GetPagedList([FromQuery] PageQueryViewModel pageQuery)
        {
            var query = new RoleGetPagedListQuery
            {
                Page = pageQuery.Page,
                PageSize = pageQuery.PageSize,
                SortColumn = pageQuery.SortColumn,
                SortOrder = pageQuery.SortOrder,
                Filters = pageQuery.Filters(),
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IPagedList<RoleGetPagedListViewModel>), 200)]
        public async Task<IActionResult> GetList()
        {
            var query = new RoleGetListQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] RoleCreateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var query = new RoleGetItemQuery
            {
                Id = id,
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] RoleUpdateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new RoleDeleteCommand
            {
                Id = id,
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Permission")]
        [Authorize]
        public async Task<IActionResult> Permission([FromBody] RolePermissionUpdateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Permission/{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetPermission(Guid id)
        {
            var result = await Mediator.Send(new RoleGetPermissionListQuery
            {
                Id = id,
            });
            return Ok(result);
        }
    }
}
