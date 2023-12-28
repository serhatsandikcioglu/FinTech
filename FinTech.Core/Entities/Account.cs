using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FinTech.Core.Entities
{
    public class Account : BaseEntity <Guid>
    {
        
        public string Number { get; set; }
        public Guid ApplicationUserId { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public decimal Balance { get; set; }
    }
}
