using AutoMapper;
using FinTech.Core.Constans;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.Entities;
using FinTech.Core.Enums;
using FinTech.Core.Interfaces;
using FinTech.Core.Mapper;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Threading.Tasks;

namespace FinTech.Test
{
    public class AcountActivityServiceTest
    {
        [Fact]
        public async Task CreateAsync_WhenValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var accountActivityCreateDTO = new AccountActivityCreateDTO
            {
                Amount = 100,
                TransactionType = TransactionType.Deposit
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var httpContextData = new Mock<IHttpContextData>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountActivityRepositoryMock = new Mock<IAccountActivityRepository>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<AccountActivity>(It.IsAny<AccountActivityCreateDTO>()))
    .Returns((AccountActivityCreateDTO input) =>
    {
        return new AccountActivity
        {
            Amount = input.Amount,
            TransactionType = input.TransactionType
        };
    });
            mapperMock.Setup(m => m.Map<AccountActivityDTO>(It.IsAny<AccountActivity>()))
    .Returns((AccountActivity input) =>
    {
        return new AccountActivityDTO
        {
            Amount = input.Amount,
            TransactionType = input.TransactionType
        };
    });
            unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(accountRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.AccountActivityRepository).Returns(accountActivityRepositoryMock.Object);

            var account = new Account
            {
                Id = accountId,
            };

            accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(account);

            var service = new AccountActivityService(unitOfWorkMock.Object, mapperMock.Object,httpContextData.Object);

            // Act
            var result = await service.CreateAsync(accountId, accountActivityCreateDTO);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal(accountActivityCreateDTO.Amount, result.Data.Amount);
            Assert.Equal(accountActivityCreateDTO.TransactionType, result.Data.TransactionType);

            unitOfWorkMock.Verify(uow => uow.AccountRepository.GetByIdAsync(accountId), Times.Once);
            unitOfWorkMock.Verify(uow => uow.AccountActivityRepository.AddAsync(It.IsAny<AccountActivity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
