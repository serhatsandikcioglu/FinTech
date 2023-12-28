using FinTech.Core.Interfaces;
using FinTech.Core.Mapper;
using FinTech.Infrastructure.Database;
using FinTech.Infrastructure.Repositories;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTech.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountActivityRepository, AccountActivityRepository>();
            services.AddScoped<IAccountActivityService, AccountActivityService>();
            services.AddScoped<IMoneyTransferRepository,MoneyTransferRepository>();
            services.AddScoped<IMoneyTransferService,MoneyTransferService>();
            return services;
        }
    }
}
