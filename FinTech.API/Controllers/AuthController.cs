using FinTech.Core.DTOs.Authentication;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CustomResponse<TokenDTO>>> Login(LoginDTO loginDTO)
        {
            return CreateActionResultInstance(await _authService.Login(loginDTO));
        }
    }
}
