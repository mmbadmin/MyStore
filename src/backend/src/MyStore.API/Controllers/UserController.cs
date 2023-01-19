namespace MyStore.API.Controllers
{
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RExtension;
    using MyStore.Application.Commons;
    using MyStore.Application.Users.Commands.Active;
    using MyStore.Application.Users.Commands.ChangePassword;
    using MyStore.Application.Users.Commands.Create;
    using MyStore.Application.Users.Commands.Deactive;
    using MyStore.Application.Users.Commands.Delete;
    using MyStore.Application.Users.Commands.Login;
    using MyStore.Application.Users.Commands.ResetPassword;
    using MyStore.Application.Users.Commands.Unlock;
    using MyStore.Application.Users.Commands.Update;
    using MyStore.Application.Users.Commands.UserRole;
    using MyStore.Application.Users.Queries.GetData;
    using MyStore.Application.Users.Queries.GetItem;
    using MyStore.Application.Users.Queries.GetPagedList;
    using MyStore.Application.Users.Queries.GetRoles;
    using MyStore.Common.API.Models;
    using MyStore.Common.API.Mvc;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserController : BaseController
    {
        private readonly ICacheManager<string> cache;

        public UserController(ICacheManager<string> cache)
        {
            this.cache = cache;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand command)
        {
            //var temp = command.Captcha.SplitNoneEmpty("|").ToList();
            //if (temp.Count() != 2)
            //{
            //    throw BaseException.BadRequest(Texts.User.InvalidLengthCaptcha);
            //}
            //var captchaID = temp[0];
            //var cacheItem = cache.Get($"captcha{captchaID.ToString().ToLower()}");
            //if (cacheItem != temp[1])
            //{
            //    cache.Remove($"captcha{captchaID.ToString().ToLower()}");
            //    throw BaseException.BadRequest(Texts.User.InvalidCaptcha);
            //}
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetCaptcha")]
        public IActionResult GetCaptcha()
        {
            var captcha = Captcha.New();
            var id = Guid.NewGuid();
            var item = new CacheItem<string>($"captcha{id.ToString().ToLower()}", captcha.Value.ToString(), ExpirationMode.Absolute, TimeSpan.FromMinutes(10));
            cache.Add(item);
            return Ok(new { file = captcha.Image, ID = id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] UserCreateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Active")]
        [Authorize]
        public async Task<IActionResult> Active([FromBody] UserActiveCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Deactive")]
        [Authorize]
        public async Task<IActionResult> Deactive([FromBody] UserDeactiveCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Unlock")]
        [Authorize]
        public async Task<IActionResult> Unlock([FromBody] UserUnlockCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordCommand command)
        {
            command.Id = UserId;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Data")]
        [Authorize]
        public async Task<IActionResult> UserData()
        {
            var result = await Mediator.Send(new UserGetDataQuery { UserId = UserId });
            return Ok(result);
        }

        [HttpPost("Role")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] UserRoleUpdateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var query = new UserGetItemQuery
            {
                Id = id,
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Filter")]
        [Authorize]
        [ProducesResponseType(typeof(IPagedList<UserGetPagedListViewModel>), 200)]
        public async Task<IActionResult> GetPagedList([FromQuery] PageQueryViewModel pageQuery)
        {
            var query = new UserGetPagedListQuery
            {
                Page = pageQuery.Page,
                PageSize = pageQuery.PageSize,
                SortOrder = pageQuery.SortOrder,
                SortColumn = pageQuery.SortColumn,
                Filters = pageQuery.Filters(),
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Role/{id:Guid}")]
        [Authorize]
        [ProducesResponseType(typeof(List<Guid>), 200)]
        public async Task<IActionResult> GetRoles(Guid id)
        {
            var query = new UserGetRoleQuery
            {
                Id = id,
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new UserDeleteCommand
            {
                Id = id,
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
