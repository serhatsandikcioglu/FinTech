using FinTech.Core.DTOs.Account;
using FinTech.Core.DTOs.AccountActivity;
using FinTech.Core.DTOs.Authentication;
using FinTech.Core.DTOs.Balance;
using FinTech.Core.DTOs.MoneyTransfer;
using FinTech.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinTech.Test
{
    public class BankAccountOperationsIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        public BankAccountOperationsIntegrationTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task BankAccountOperations_WhenValidData_ReturnSuccessResponse()
        {
            var client = _factory.CreateClient();
            //Login
            LoginDTO loginDTO = new LoginDTO
            {
                IdentityNumber = "123",
                Password = "Asd123*"
            };

            var loginDTOJsonContent = JsonSerializer.Serialize(loginDTO);
            var loginDTOContent = new StringContent(loginDTOJsonContent, Encoding.UTF8, "application/json");

            var loginResponse = await client.PostAsync("api/Auth/login", loginDTOContent);
            loginResponse.EnsureSuccessStatusCode();

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();

            var loginCustomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponse<TokenDTO>>(loginResponseContent);
            var token = loginCustomResponse.Data.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Create New Account
            AccountCreateDTO accountCreateDTO = new AccountCreateDTO
            {
                Balance = 5000,
                SenderAccountId = new Guid("7bb1346a-ec88-4e45-a6bc-54844212444e")
            };
            var accountCreateDTOjsonContent = JsonSerializer.Serialize(accountCreateDTO);
            var accountCreateDTOcontent = new StringContent(accountCreateDTOjsonContent, Encoding.UTF8, "application/json");

            var accountCreateResponse = await client.PostAsync("api/accounts", accountCreateDTOcontent);


            accountCreateResponse.EnsureSuccessStatusCode();

            var accountCreateResponseContent = await accountCreateResponse.Content.ReadAsStringAsync();

            var customResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponse<AccountDTO>>(accountCreateResponseContent);
            var accountDTO = customResponse.Data;

            Assert.NotNull(accountDTO.Number);
            Assert.True(accountCreateResponse.IsSuccessStatusCode);

            //Deposit Money
            AccountActivityCreateDTO accountActivityCreateDTO = new AccountActivityCreateDTO
            {
                Amount = 500,
                TransactionType = Core.Enums.TransactionType.Deposit,

            };

            var accountActivityCreateDTOJsonContent = JsonSerializer.Serialize(accountActivityCreateDTO);
            var accountActivityCreateDTOContent = new StringContent(accountActivityCreateDTOJsonContent, Encoding.UTF8, "application/json");

            var accountActivityResponse = await client.PostAsync($"api/accountActivities/{accountDTO.Id}", accountActivityCreateDTOContent);

            accountActivityResponse.EnsureSuccessStatusCode();

            var accountActivityResponseContent = await accountActivityResponse.Content.ReadAsStringAsync();

            var accountActivityCustomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponse<AccountActivityDTO>>(accountActivityResponseContent);
            var accountActivityDTO = accountActivityCustomResponse.Data;

            Assert.True(accountActivityResponse.IsSuccessStatusCode);
            Assert.Equal(accountActivityDTO.Date.Date, DateTime.UtcNow.Date);
            Assert.Equal(accountActivityDTO.Amount, accountActivityCreateDTO.Amount);
            Assert.Equal(accountActivityDTO.TransactionType, accountActivityCreateDTO.TransactionType);

            // Transfer
            ExternalTransferCreateDTO externalTransferCreateDTO = new ExternalTransferCreateDTO
            {
                Amount = 300,
                Name = "Receiver Name",
                Surname = "Receiver Surname",
                ReceiverAccountNumber = "123123",
                SenderAccountId = accountDTO.Id
            };

            var externalTransferCreateDTOJsonContent = JsonSerializer.Serialize(externalTransferCreateDTO);
            var externalTransferCreateDTOContent = new StringContent(externalTransferCreateDTOJsonContent, Encoding.UTF8, "application/json");

            var externalTransferResponse = await client.PostAsync("api/moneyTransfers/externalTransfer", externalTransferCreateDTOContent);

            externalTransferResponse.EnsureSuccessStatusCode();

            Assert.True(externalTransferResponse.IsSuccessStatusCode);

            //Check Balance Of Both Accounts
            var senderAccountBalanceResponse = await client.GetAsync($"api/accounts/balance/{accountDTO.Id}");

            senderAccountBalanceResponse.EnsureSuccessStatusCode();

            var senderAccountBalanceResponseContent = await senderAccountBalanceResponse.Content.ReadAsStringAsync();

            var senderAccountBalanceCustomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponse<BalanceDTO>>(senderAccountBalanceResponseContent);
            var senderAccountBalanceDTO = senderAccountBalanceCustomResponse.Data;


            var receiverAccountBalanceResponse = await client.GetAsync($"api/accounts/balance/{new Guid("09e1b4cd-5838-4c66-ab39-ed190a178c52")}");

            receiverAccountBalanceResponse.EnsureSuccessStatusCode();

            var receiverAccountBalanceResponseContent = await receiverAccountBalanceResponse.Content.ReadAsStringAsync();

            var receiverAccountBalanceCustomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponse<BalanceDTO>>(receiverAccountBalanceResponseContent);
            var receiverAccountBalanceDTO = receiverAccountBalanceCustomResponse.Data;


            Assert.Equal(externalTransferCreateDTO.Amount, receiverAccountBalanceDTO.Balance);
            Assert.Equal(accountCreateDTO.Balance + accountActivityCreateDTO.Amount - externalTransferCreateDTO.Amount, senderAccountBalanceDTO.Balance);
        }
    }
}
