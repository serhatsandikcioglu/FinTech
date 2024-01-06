using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Interfaces
{
    public interface IHttpContextData
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Token { get; }
        List<string> UserRoleNames { get; }
        string RequestUrl { get; }
    }
}
