﻿using FinTech.Core.Constans;
using FinTech.Core.DTOs.AutomaticPayment;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Entities;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTech.API.Controllers
{
    [Route("api/automaticPayments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "customer,admin")]
    public class AutomaticPaymentController : CustomBaseController
    {
        private readonly IAutomaticPaymentService _automaticPaymentService;

        public AutomaticPaymentController(IAutomaticPaymentService automaticPaymentService)
        {
            _automaticPaymentService = automaticPaymentService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomResponse<AutomaticPaymentDTO>>> Create(AutomaticPaymentCreateDTO automaticPaymentCreateDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return CreateActionResultInstance(await _automaticPaymentService.CreateAsync(userId,automaticPaymentCreateDTO));
        }
        [HttpDelete("{automaticPaymentId}")]
        public async Task<ActionResult<CustomResponse<NoContent>>> Delete(Guid automaticPaymentId)
        {
            return CreateActionResultInstance(await _automaticPaymentService.DeleteAsync(automaticPaymentId));
        }
        [HttpGet("byUser")]
        public async Task<ActionResult<CustomResponse<List<AutomaticPaymentDTO>>>> GetAllByUserId()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return CreateActionResultInstance(await _automaticPaymentService.GetAllByUserIdAsync(userId));
        }
        [HttpPost("bill")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomResponse<NoContent>>> CreateBill(Bill bill) //This endpoint is for the automatic payment order test.
        {
            return CreateActionResultInstance(await _automaticPaymentService.CreateBill(bill));
        }
    }
}
