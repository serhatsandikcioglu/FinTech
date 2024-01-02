using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.Enums
{
    public enum TicketStatus
    {
        Pending = 0,
        Processing = 1,
        Solved = 2,
        Unsolved = 3
    }
    public enum TicketPriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
