using AutoMapper;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Service.Interfaces;
using FinTech.Shared.Constans;
using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FinTech.Core.DTOs.LoanApplication;
using FinTech.Core.Constans;

namespace FinTech.Service.Services
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LoanApplicationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        private async Task<string> CalculateCreditScoreAsync(Guid applicationUserId, decimal monthlyIncome)
        {
            try
            {
                var incomeRatio = Math.Min(1, monthlyIncome / LoanApplicationConstants.MaxIncome);
                var paymentStatus = await _unitOfWork.LoanApplicationRepository.GetPaymentStatusesByUserIdAsync(applicationUserId);

                var delayedPaymentCount = paymentStatus.Count(x => x == Core.Enums.PaymentStatus.DelayedPayment);
                var loanRepaymentHabitsRatio = (paymentStatus.Count == 0) ? 0 : 1 - (delayedPaymentCount / paymentStatus.Count);

                var creditScore = ((incomeRatio * 0.35m) + (loanRepaymentHabitsRatio * 0.65m)) * 1900;

                if (creditScore >= 1700)
                {
                    return "Very Good";
                }
                else if (creditScore > 1500 && creditScore <= 1699)
                {
                    return "Good";
                }
                else if (creditScore > 1100 && creditScore <= 1499)
                {
                    return "Less Risky";
                }
                else if (creditScore > 700 && creditScore <= 1099)
                {
                    return "Moderate Risk";
                }
                else
                {
                    return "Most Risky";
                }
            }
            catch (Exception)
            {
                return "Not Calculated";
            }
                
        }

        public async Task<CustomResponse<LoanApplicationDTO>> CreateAsync(Guid applicationUserId, LoanApplicationCreateDTO loanApplicationCreateDTO)
        {
            LoanApplication loanApplication = _mapper.Map<LoanApplication>(loanApplicationCreateDTO);
            loanApplication.Status = Core.Enums.LoanApllicationStatus.Pending;
            loanApplication.Date = DateTime.UtcNow;
            loanApplication.ApplicationUserId = applicationUserId;
            loanApplication.CreditScoreResultComment = await CalculateCreditScoreAsync(applicationUserId, loanApplicationCreateDTO.MonthlyIncome);

            await _unitOfWork.LoanApplicationRepository.AddAsync(loanApplication);
            LoanApplicationDTO loanApplicationDTO = _mapper.Map<LoanApplicationDTO>(loanApplication);
            await _unitOfWork.SaveChangesAsync();
            return CustomResponse<LoanApplicationDTO>.Success(StatusCodes.Status201Created, loanApplicationDTO);
        }
        public async Task<CustomResponse<NoContent>> LoanApplicationEvaluationAsync(Guid loanApplicationId, LoanApplicationEvaluationDTO loanApplicationEvaluationDTO)
        {
            LoanApplication loanApplication = await _unitOfWork.LoanApplicationRepository.GetByIdAsync(loanApplicationId);
            if (loanApplication == null)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status404NotFound, ErrorMessageConstants.LoanApplicationNotFound);
            if (loanApplication.Status != Core.Enums.LoanApllicationStatus.Pending)
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, ErrorMessageConstants.LoanApplicationProcessed);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                loanApplication.Status = loanApplicationEvaluationDTO.LoanApllicationStatus;
                await _unitOfWork.SaveChangesAsync();

                if (loanApplication.Status == Core.Enums.LoanApllicationStatus.Approved)
                {
                    await CreateRepaymentPlanAsync(loanApplicationId, loanApplication.Amount, loanApplication.MaturityTerm);
                }
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }

        }
        private async Task<CustomResponse<NoContent>> CreateRepaymentPlanAsync(Guid loanApplicationId, decimal amount, int maturityTerm)
        {
            try
            {
                var InterestFactor = Math.Pow(1 + LoanApplicationConstants.MonthlyInterestRate, maturityTerm);
                var MonthlyPayment = (((decimal)InterestFactor * (decimal)LoanApplicationConstants.MonthlyInterestRate) / ((decimal)InterestFactor - 1)) * amount;

                for (int i = 1; i <= maturityTerm; i++)
                {
                    RepaymentPlan repaymentPlan = new RepaymentPlan
                    {
                        LoanApplicationId = loanApplicationId,
                        AmountDue = MonthlyPayment,
                        PaymentStatus = Core.Enums.PaymentStatus.Unpaid,
                        InstallmentNumber = i,
                        DueDate = DateTime.UtcNow.AddMonths(i)
                    };
                    await _unitOfWork.RepaymentPlanRepository.AddAsync(repaymentPlan);
                }

                await _unitOfWork.SaveChangesAsync();
                return CustomResponse<NoContent>.Success(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return CustomResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, new List<string> { ex.Message });
            }
            
        }
        public async Task<CustomResponse<List<LoanApplicationDTO>>> GetAllByUserId(Guid userId)
        {
           List<LoanApplication> loanApplications =  await _unitOfWork.LoanApplicationRepository.GetAllByUserIdAsync(userId);

            if (loanApplications == null)
                return CustomResponse<List<LoanApplicationDTO>>.Fail(StatusCodes.Status404NotFound,ErrorMessageConstants.LoanApplicationNotFound);

            List<LoanApplicationDTO> loanApplicationDTOs = _mapper.Map<List<LoanApplicationDTO>>(loanApplications);
            return CustomResponse<List<LoanApplicationDTO>>.Success(StatusCodes.Status200OK,loanApplicationDTOs);
        }
    }
}
