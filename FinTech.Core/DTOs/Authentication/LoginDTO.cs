using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.Authentication
{
    public class LoginDTO
    {
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
    }
}
