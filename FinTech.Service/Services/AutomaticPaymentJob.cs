using FinTech.Service.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Services
{
    public class AutomaticPaymentJob : IJob
    {
        private readonly IAutomaticPaymentService _automaticPaymentService;

        public AutomaticPaymentJob(IAutomaticPaymentService automaticPaymentService)
        {
            _automaticPaymentService = automaticPaymentService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _automaticPaymentService.ProcessAllAutomaticPaymentsAsync();
        }
    }
}
