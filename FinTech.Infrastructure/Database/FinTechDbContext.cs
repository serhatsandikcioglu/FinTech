using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FinTech.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure.Database
{
    public class FinTechDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public FinTechDbContext(DbContextOptions<FinTechDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AutomaticPayment> AutomaticPayments { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
