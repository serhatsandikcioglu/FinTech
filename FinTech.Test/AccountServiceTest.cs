using AutoMapper;
using FinTech.Core.Constans;
using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.DTOs.Balance;
using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Shared.Constans;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Test
{
    public class AccountServiceTest
    {
        [Fact]
        public async Task CreateAccountAsync_WhenValidData_ReturnSuccessResponse()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();
            var httpContextData = new Mock<IHttpContextData>();

            IAccountService accountService = new AccountService(mapperMock.Object, unitOfWorkMock.Object, accountActivityServiceMock.Object, httpContextData.Object);

            Account initialAccount = new Account
            {
                Id = Guid.NewGuid(),
                Number = "000001",
                AccountActivities = new List<AccountActivity>
                {
                    new AccountActivity
                    {
                    Amount = 20000,
                    TransactionType = Core.Enums.TransactionType.Deposit

                    }
                },
                ApplicationUserId = Guid.NewGuid()
            };

            AccountCreateDTO accountCreateDTO = new AccountCreateDTO
            {
                Balance = 10000,
                SenderAccountId = initialAccount.Id
            };

            AccountActivityCreateDTO senderAccountActivityCreateDTO = new AccountActivityCreateDTO
            {
                Amount = accountCreateDTO.Balance,
                TransactionType = Core.Enums.TransactionType.Deposit
            };

            httpContextData.Setup(h => h.UserId).Returns(Guid.NewGuid().ToString());

            mapperMock.Setup(m => m.Map<Account>(It.IsAny<AccountCreateDTO>()))
            .Returns((AccountCreateDTO input) =>
            {
                return new Account
                {

                };
            });

            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetBiggestAccountNumberAsync()).ReturnsAsync(initialAccount.Number);

            mapperMock.Setup(m => m.Map<AccountDTO>(It.IsAny<Account>()))
            .Returns((Account input) =>
            {
                return new AccountDTO
                {
                    Id = input.Id,
                    Number = input.Number
                };
            });

            accountActivityServiceMock.Setup(ser => ser.CreateAsync(It.IsAny<Guid>(), It.IsAny<AccountActivityCreateDTO>(), null))
                .ReturnsAsync(new CustomResponse<AccountActivityDTO>
                {
                    Error = null
                });

            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(initialAccount);

            var result = await accountService.CreateAccountAccordingRulesAsync(accountCreateDTO);
            var accountDTO = result.Data;

            Assert.NotNull(accountDTO.Number);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        [Fact]
        public async Task CreateAccountAsync_MinimumBalanceCheck_ReturnsFailureResponse()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();
            var httpContextData = new Mock<IHttpContextData>();

            IAccountService accountService = new AccountService(mapperMock.Object, unitOfWorkMock.Object, accountActivityServiceMock.Object, httpContextData.Object);

            Account initialAccount = new Account
            {
                Id = Guid.NewGuid(),
                Number = "000001",
                AccountActivities = new List<AccountActivity>
                {
                    new AccountActivity
                    {
                    Amount = 20000,
                    TransactionType = Core.Enums.TransactionType.Deposit

                    }
                },
                ApplicationUserId = Guid.NewGuid()
            };

            AccountCreateDTO accountCreateDTO = new AccountCreateDTO
            {
                Balance = 1000,
                SenderAccountId = initialAccount.Id
            };

            var result = await accountService.CreateAccountAccordingRulesAsync(accountCreateDTO);

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(result.Error.Details[0], $"{ErrorMessageConstants.InitialBalanceError}. Minimum Balance {AccountConstants.MinimumInitialBalance}");
        }
        [Fact]
        public async Task GetBalanceAsync_WhenValidData_ReturnsSuccessResponse()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();
            var httpContextData = new Mock<IHttpContextData>();

            IAccountService accountService = new AccountService(mapperMock.Object, unitOfWorkMock.Object, accountActivityServiceMock.Object, httpContextData.Object);

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                Number = "000001",
                AccountActivities = new List<AccountActivity>
                {
                    new AccountActivity
                    {
                    Amount = 20000,
                    TransactionType = Core.Enums.TransactionType.Deposit

                    },new AccountActivity
                    {
                    Amount = 10000,
                    TransactionType = Core.Enums.TransactionType.Withdrawal

                    },new AccountActivity
                    {
                    Amount = 30000,
                    TransactionType = Core.Enums.TransactionType.Deposit

                    }
                },
                ApplicationUserId = Guid.NewGuid()
            };

            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetUserIdById(account.Id)).ReturnsAsync(account.ApplicationUserId);
            unitOfWorkMock.Setup(repo => repo.AccountRepository.AccountIsExistAsync(account.Id)).ReturnsAsync(true);
            unitOfWorkMock.Setup(repo => repo.AccountActivityRepository.GetAllByAccountIdAsync(account.Id)).ReturnsAsync(account.AccountActivities);
            httpContextData.Setup(h => h.UserId).Returns(account.ApplicationUserId.ToString());

            var result = await accountService.GetBalanceByAccountIdAsync(account.Id);
            var balanceDTO = result.Data;


            Assert.True(result.Succeeded);
            Assert.Equal(20000 - 10000 + 30000, balanceDTO.Balance);
        }
        [Fact]
        public async Task UpdateBalanceAsync_WhenValidData_ReturnsSuccessResponse()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();
            var httpContextData = new Mock<IHttpContextData>();

            IAccountService accountService = new AccountService(mapperMock.Object, unitOfWorkMock.Object, accountActivityServiceMock.Object, httpContextData.Object);

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                Number = "000001",
                AccountActivities = new List<AccountActivity>
                {
                    new AccountActivity
                    {
                    Amount = 20000,
                    TransactionType = Core.Enums.TransactionType.Deposit

                    }
                },
                ApplicationUserId = Guid.NewGuid()
            };

            BalanceUpdateDTO balanceUpdateDTO = new BalanceUpdateDTO
            {
                Balance = 50000
            };

            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetByIdAsync(account.Id)).ReturnsAsync(account);
            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetUserIdById(account.Id)).ReturnsAsync(account.ApplicationUserId);
            unitOfWorkMock.Setup(repo => repo.AccountRepository.AccountIsExistAsync(account.Id)).ReturnsAsync(true);
            unitOfWorkMock.Setup(repo => repo.AccountActivityRepository.GetAllByAccountIdAsync(account.Id)).ReturnsAsync(account.AccountActivities);
            httpContextData.Setup(h => h.UserId).Returns(account.ApplicationUserId.ToString());

            accountActivityServiceMock.Setup(ser => ser.CreateAsync(It.IsAny<Guid>(), It.IsAny<AccountActivityCreateDTO>(), null))
                .ReturnsAsync(new CustomResponse<AccountActivityDTO>
                {
                    Error = null
                });

            mapperMock.Setup(m => m.Map<BalanceDTO>(It.IsAny<BalanceUpdateDTO>()))
            .Returns((BalanceUpdateDTO input) =>
            {
                return new BalanceDTO
                {
                    Balance = input.Balance
                };
            });


            var result = await accountService.UpdateBalanceAsync(account.Id,balanceUpdateDTO);


            Assert.True(result.Succeeded);
            Assert.Equal(balanceUpdateDTO.Balance,result.Data.Balance);

        }
    }
}
