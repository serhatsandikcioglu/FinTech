using FinTech.Core.Constans;
using FinTech.Core.Entities;
using FinTech.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Test
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<FinTechDbContext>));
                services.Remove(serviceDescriptor);

                services.AddDbContext<FinTechDbContext>(options =>
                {
                    options.UseInMemoryDatabase("FinTech");
                    options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FinTechDbContext>();

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                ApplicationUser senderUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Name = "Sender Name",
                    Surname = "Sender Surname",
                    UserName = "123",
                    Address = "Test",
                    IdentityNumber = "123",
                    Accounts = new List<Account> { new Account
                    {
                        Id =new Guid("7bb1346a-ec88-4e45-a6bc-54844212444e"),
                        Number = "000001",
                        AccountActivities = new List<AccountActivity> { new AccountActivity
                        {
                            Amount = 20000,
                            TransactionType = Core.Enums.TransactionType.Deposit,
                            
                        } }
                    }
                }
                };

                ApplicationUser receiverUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Name = "Receiver Name",
                    Surname = "Receiver Surname",
                    UserName = "1234",
                    Address = "Test",
                    IdentityNumber = "1234",
                    Accounts = new List<Account> { new Account
                {
                        Id = new Guid("09e1b4cd-5838-4c66-ab39-ed190a178c52"),
                    Number = "123123",

                } }
                };

                roleManager.CreateAsync(new() { Name = "customer"});
                roleManager.CreateAsync(new() { Name = "admin"});

                userManager.CreateAsync(senderUser, "Asd123*");
                userManager.AddToRoleAsync(senderUser, "customer");
                userManager.AddToRoleAsync(senderUser, "admin");

                userManager.CreateAsync(receiverUser, "Asd123*");
                userManager.AddToRoleAsync(receiverUser, "customer");

                db.SaveChanges();
                builder.UseEnvironment("Development");
            });
        }
    }
}
