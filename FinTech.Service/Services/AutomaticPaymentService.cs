using AutoMapper;
using FinTech.Core.Constans;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.DTOs.AutomaticPayment;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Service.Services
{
    public class AutomaticPaymentService : IAutomaticPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountActivityService _accountActivityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextData _httpContextData;

        public AutomaticPaymentService(IUnitOfWork unitOfWork, IAccountActivityService accountActivityService, IMapper mapper, IHttpContextData httpContextData)
        {
            _unitOfWork = unitOfWork;
            _accountActivityService = accountActivityService;
            _mapper = mapper;
            _httpContextData = httpContextData;
        }

        public async Task<CustomResponse<AutomaticPaymentDTO>> CreateAsync(AutomaticPaymentCreateDTO automaticPaymentCreateDTO)
        {
            bool accountExist = await _unitOfWork.AccountRepository.AccountIsExistAsync(automaticPaymentCreateDTO.AccountId);
            if (!accountExist)
                return CustomResponse<AutomaticPaymentDTO>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.AccountNotFound);

            AutomaticPayment automaticPayment = _mapper.Map<AutomaticPayment>(automaticPaymentCreateDTO);
            automaticPayment.userId = Guid.Parse(_httpContextData.UserId!);
            await _unitOfWork.AutomaticPaymentRepository.AddAsync(automaticPayment);
            await _unitOfWork.SaveChangesAsync();

            AutomaticPaymentDTO automaticPaymentDTO = _mapper.Map<AutomaticPaymentDTO>(automaticPayment);
            return CustomResponse<AutomaticPaymentDTO>.Success(StatusCodes.Status201Created, automaticPaymentDTO);
        }

        public async Task<CustomResponse<NoContent>> DeleteAsync(Guid automaticPaymentId)
        {
            bool automaticPaymentExist = await _unitOfWork.AutomaticPaymentRepository.IsExistAsync(automaticPaymentId);
            if (!automaticPaymentExist)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound,ErrorMessageConstants.AutomaticPaymentNotFound);

            await _unitOfWork.AutomaticPaymentRepository.DeleteAsync(automaticPaymentId);
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<List<AutomaticPaymentDTO>>> GetAllByUserIdAsync()
        {
           var automaticPayments = await _unitOfWork.AutomaticPaymentRepository.GetAllByUserIdAsync(Guid.Parse(_httpContextData.UserId!));
            if (automaticPayments == null)
            {
                return CustomResponse<List<AutomaticPaymentDTO>>.Fail(StatusCodes.Status200OK,ErrorMessageConstants.AutomaticPaymentNotFound);
            }
            List<AutomaticPaymentDTO> automaticPaymentDTOs = _mapper.Map<List<AutomaticPaymentDTO>>(automaticPayments);
            return CustomResponse<List<AutomaticPaymentDTO>>.Success(StatusCodes.Status200OK, automaticPaymentDTOs);
        }

        public async Task<CustomResponse<NoContent>> ProcessAllAutomaticPaymentsAsync()
        {
            var automaticPayments = await _unitOfWork.AutomaticPaymentRepository.GetAllAsync();

            foreach (var automaticPayment in automaticPayments)
            {
                var bills = await _unitOfWork.BillRepository.GetByNumberAsync(automaticPayment.BillNumber);
                var userId = automaticPayment.userId;

                foreach (var bill in bills)
                {
                    try
                    {
                        if (bill != null)
                        {
                            await _unitOfWork.BeginTransactionAsync();

                            AccountActivityCreateDTO accountActivityCreateDTO = new AccountActivityCreateDTO
                            {
                                Amount = bill.Amount,
                                TransactionType = Core.Enums.TransactionType.Withdrawal
                            };
                            var paymentResponse = await _accountActivityService.CreateAsync(automaticPayment.AccountId, accountActivityCreateDTO,userId);

                            if (paymentResponse.Error == null)
                                bill.IsPaid = true;

                            await _unitOfWork.SaveChangesAsync();
                            await _unitOfWork.CommitAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.RollbackAsync();
                        return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, $"An error occurred while paying all owed bills: {ex.Message}");
                    }
                }
            }
            return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
        }
        public async Task<CustomResponse<NoContent>> CreateBill(Bill bill) //This method is for the automatic payment order test.
        {
            bill.Id = Guid.NewGuid();
            await _unitOfWork.BillRepository.AddAsync(bill);
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<NoContent>.Success(StatusCodes.Status201Created);
        }
    }

}
