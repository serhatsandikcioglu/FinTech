using FinTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.User
{
    public class UserCreateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public int MonthlyIncome { get; set; }
    }
}
