using FinTech.Core.Constans;
using FinTech.Core.DTOs.Authentication;
using FinTech.Core.DTOs.Role;
using FinTech.Core.DTOs.User;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = RoleConstants.CustomerSupport)]
        public async Task<ActionResult<CustomResponse<UserDTO>>> CreateCustomer(UserCreateDTO userCreateDTO)
        {
            return CreateActionResultInstance(await _userService.CreateCustomer(userCreateDTO));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = RoleConstants.Manager)]
        public async Task<ActionResult<CustomResponse<UserDTO>>> CreateUser(UserCreateDTO userCreateDTO)
        {
            return CreateActionResultInstance(await _userService.CreateUser(userCreateDTO));
        }
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = RoleConstants.Manager)]
        public async Task<ActionResult<CustomResponse<UserRolesDTO>>> GetRolesFromUser(Guid userId)
        {
            return CreateActionResultInstance(await _userService.GetRoles(userId));
        }
        [HttpDelete("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = RoleConstants.Manager)]
        public async Task<ActionResult<CustomResponse<NoContent>>> DeleteRoleFromUser(Guid userId, RoleDTO roleDTO)
        {
            return CreateActionResultInstance(await _userService.DeleteRole(userId,roleDTO));
        }
        [HttpPost("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = RoleConstants.Manager)]
        public async Task<ActionResult<CustomResponse<NoContent>>> AddRoleToUser(Guid userId, RoleDTO roleDTO)
        {
            return CreateActionResultInstance(await _userService.AssignRole(userId,roleDTO));
        }
    }
}
