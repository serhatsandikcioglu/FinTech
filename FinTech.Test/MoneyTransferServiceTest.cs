using AutoMapper;
using FinTech.Core.Constans;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Models;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using FinTech.Shared.Constans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Test
{
    public class MoneyTransferServiceTest
    {
        [Fact]
        public async Task PerformMoneyTransferChecks_DailyLimitExceeded_ReturnsFailureResponse()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();

            IMoneyTransferService moneyTransferService = new MoneyTransferService(unitOfWorkMock.Object, mapperMock.Object, accountActivityServiceMock.Object);

            var externalTransferCreateDTO = new ExternalTransferCreateDTO
            {
                Amount = MoneyTransferConstants.PerTransactionTransferLimit + 1,
                Name = "ReceiverName",
                Surname = "ReceiverSurname",
                ReceiverAccountNumber = "ReceiverAccountNumber",
                SenderAccountId = Guid.NewGuid()
            };

            var receiverAccount = new Account
            {
                Id = Guid.NewGuid(),
                Number = "ReceiverAccountNumber",
                ApplicationUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Name = "ReceiverName",
                    Surname = "ReceiverSurname"
                }
            };
            mapperMock.Setup(m => m.Map<MoneyTransfer>(It.IsAny<ExternalTransferCreateDTO>()))
            .Returns((ExternalTransferCreateDTO input) =>
            {
                return new MoneyTransfer
                {
                    Amount = input.Amount,
                    ReceiverAccountId = receiverAccount.Id,
                    SenderAccountId = externalTransferCreateDTO.SenderAccountId
                };
            });

            unitOfWorkMock.Setup(repo => repo.MoneyTransferRepository.GetDailyTransferAmountAsync(It.IsAny<Guid>(), DateTime.UtcNow.Date)).ReturnsAsync(new List<decimal> { externalTransferCreateDTO.Amount });
            unitOfWorkMock.Setup(repo => repo.AccountRepository.AccountIsExistAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetByAccountNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(receiverAccount);

            unitOfWorkMock.Setup(repo => repo.MoneyTransferRepository.AddAsync(It.IsAny<MoneyTransfer>()));


            // Act
            var result = await moneyTransferService.ExternalTransferAsync(externalTransferCreateDTO);

            // Assert
            Assert.True(!result.Succeeded);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal($"{ErrorMessageConstants.TransferLimitError}. Daily limit:{ErrorMessageConstants.TransferLimitError}", result.Error.Details[0]);
        }
        [Fact]
        public async Task PerformMoneyTransferChecks_WhenValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var accountActivityServiceMock = new Mock<IAccountActivityService>();

            IMoneyTransferService moneyTransferService = new MoneyTransferService(unitOfWorkMock.Object, mapperMock.Object, accountActivityServiceMock.Object);


            var receiverAccount = new Account
            {
                Id = Guid.NewGuid(),
                Number = "ReceiverAccountNumber",
                ApplicationUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Name = "ReceiverName",
                    Surname = "ReceiverSurname"
                }
            };

            var senderAccount = new Account
            {
                Id = Guid.NewGuid(),
                AccountActivities = new List<AccountActivity> { 
                    new AccountActivity {
                        Id = Guid.NewGuid(),
                        Amount = 10000,
                        TransactionType = Core.Enums.TransactionType.Deposit
                } }
            };
            var externalTransferCreateDTO = new ExternalTransferCreateDTO
            {
                Amount = 5000,
                Name = "ReceiverName",
                Surname = "ReceiverSurname",
                ReceiverAccountNumber = "ReceiverAccountNumber",
                SenderAccountId = senderAccount.Id
            };

            mapperMock.Setup(m => m.Map<MoneyTransfer>(It.IsAny<ExternalTransferCreateDTO>()))
            .Returns((ExternalTransferCreateDTO input) =>
            {
                return new MoneyTransfer
                {
                    Amount = input.Amount,
                    ReceiverAccountId = receiverAccount.Id,
                    SenderAccountId = externalTransferCreateDTO.SenderAccountId
                };
            });

            unitOfWorkMock.Setup(repo => repo.MoneyTransferRepository.GetDailyTransferAmountAsync(It.IsAny<Guid>(), DateTime.UtcNow.Date)).ReturnsAsync(new List<decimal> { externalTransferCreateDTO.Amount });
            unitOfWorkMock.Setup(repo => repo.AccountRepository.AccountIsExistAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(repo => repo.AccountRepository.GetByAccountNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(receiverAccount);

            unitOfWorkMock.Setup(repo => repo.MoneyTransferRepository.AddAsync(It.IsAny<MoneyTransfer>()));

            accountActivityServiceMock.Setup(ser => ser.CreateAsync(It.IsAny<Guid>(), It.IsAny<AccountActivityCreateDTO>(), null))
                .ReturnsAsync(new CustomResponse<AccountActivityDTO>
                {
                    Error = null
                });
            accountActivityServiceMock.Setup(ser => ser.CreateAsync(It.IsAny<Guid>(), It.IsAny<AccountActivityCreateDTO>(), It.IsAny<Guid>()))
                .ReturnsAsync(new CustomResponse<AccountActivityDTO>
                {
                    Error = null
                });

            // Act
            var result = await moneyTransferService.ExternalTransferAsync(externalTransferCreateDTO);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
    }
}
