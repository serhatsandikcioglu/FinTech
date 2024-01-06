using FinTech.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinTech.Core.Models
{
    public class HttpContextData : IHttpContextData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<string> _userRoleNames;

        public HttpContextData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity.Name;

        public string Token => _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault()?.Split("Bearer ")[1];

        public List<string> UserRoleNames
        {
            get
            {
                if (_userRoleNames == null)
                    _userRoleNames = _httpContextAccessor.HttpContext?.User?.Claims.Where(k => k.Type == ClaimTypes.Role)
                        .Select(k => k.Value).ToList();
                return _userRoleNames;
            }
        }

        public string RequestUrl =>
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.Path}";
    }
}
