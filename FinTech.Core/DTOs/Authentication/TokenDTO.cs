using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.Authentication
{
    public class TokenDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
