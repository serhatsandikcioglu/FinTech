﻿using FinTech.Core.DTOs.AutomaticPayment;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Interfaces
{
    public interface IAutomaticPaymentService
    {
        Task<CustomResponse<NoContent>> ProcessAllAutomaticPaymentsAsync();
        Task<CustomResponse<AutomaticPaymentDTO>> CreateAsync(Guid accountId,Guid userId,AutomaticPaymentCreateDTO automaticPaymentCreateDTO);
        Task<CustomResponse<NoContent>> DeleteAsync(Guid automaticPaymentId);
        Task<CustomResponse<List<AutomaticPaymentDTO>>> GetAllByUserIdAsync(Guid userId);
        Task<CustomResponse<NoContent>> CreateBill(Bill bill);

    }
}