using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Entities
{
    public class Account : BaseEntity <Guid>
    {
        public Guid UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public decimal Balance { get; set; }
    }
}
