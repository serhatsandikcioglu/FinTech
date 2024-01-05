using FinTech.Core.Entities;
using FinTech.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.LoanApplication
{
    public class LoanApplicationDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int MaturityTerm { get; set; }
        public LoanApllicationStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
