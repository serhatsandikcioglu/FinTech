using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.User
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
    }
}
