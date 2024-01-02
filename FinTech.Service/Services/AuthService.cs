using FinTech.Core.Constans;
using FinTech.Core.DTOs.Authentication;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly CustomTokenOption _customTokenOption;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IOptions<CustomTokenOption> customTokenOption, UserManager<ApplicationUser> userManager)
        {
            _customTokenOption = customTokenOption.Value;
            _userManager = userManager;
        }
        public async Task<CustomResponse<TokenDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.IdentityNumber);
            if (user == null)
                return CustomResponse<TokenDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.IncorrectUsernameOrPassword);
            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                return CustomResponse<TokenDTO>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.IncorrectUsernameOrPassword);
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userRoles.ToList().ForEach(x =>
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            });
            var token = GenerateToken(claims);
            TokenDTO tokenDTO = new TokenDTO
            {
                Token = token,
                UserId = user.Id.ToString()
            };
            return CustomResponse<TokenDTO>.Success(StatusCodes.Status200OK, tokenDTO);
        }
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var audience = _customTokenOption.Audience[0];
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customTokenOption.SecurityKey));
            var tokenDesc = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Expires = accessTokenExpiration,
                Subject = new ClaimsIdentity(claims),
                Issuer = _customTokenOption.Issuer,
                Audience = audience
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDesc);
            return handler.WriteToken(token);
        }
    }
}
