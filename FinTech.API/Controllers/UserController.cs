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
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{RoleConstants.Manager},{RoleConstants.Admin}")]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("customer")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomResponse<UserDTO>>> CreateCustomer(UserCreateDTO userCreateDTO)
        {
            return CreateActionResultInstance(await _userService.CreateCustomerAsync(userCreateDTO));
        }
        [HttpPost("user")]
        public async Task<ActionResult<CustomResponse<UserDTO>>> CreateUser(UserCreateDTO userCreateDTO)
        {
            return CreateActionResultInstance(await _userService.CreateUserAsync(userCreateDTO));
        }
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<CustomResponse<UserRolesDTO>>> GetRolesFromUser(Guid userId)
        {
            return CreateActionResultInstance(await _userService.GetRolesAsync(userId));
        }
        [HttpDelete("[action]/{userId}")]
        public async Task<ActionResult<CustomResponse<NoContent>>> DeleteRoleFromUser(Guid userId, RoleDTO roleDTO)
        {
            return CreateActionResultInstance(await _userService.DeleteRoleAsync(userId,roleDTO));
        }
        [HttpPost("[action]/{userId}")]
        public async Task<ActionResult<CustomResponse<NoContent>>> AddRoleToUser(Guid userId, RoleDTO roleDTO)
        {
            return CreateActionResultInstance(await _userService.AssignRoleAsync(userId,roleDTO));
        }
    }
}
