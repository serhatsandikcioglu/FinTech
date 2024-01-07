using FinTech.Core.Entities;
using FinTech.Core.Interfaces;
using FinTech.Core.Mapper;
using FinTech.Core.Models;
using FinTech.Infrastructure.Database;
using FinTech.Infrastructure.Repositories;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using FinTech.Service.Validator;

namespace FinTech.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAplication(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountActivityRepository, AccountActivityRepository>();
            services.AddScoped<IAccountActivityService, AccountActivityService>();
            services.AddScoped<IMoneyTransferRepository,MoneyTransferRepository>();
            services.AddScoped<IMoneyTransferService,MoneyTransferService>();
            services.AddScoped<ILoanApplicationRepository,LoanApplicationRepository>();
            services.AddScoped<ILoanApplicationService,LoanApplicationService>();
            services.AddScoped<IRepaymentPlanRepository,RepaymentPlanRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<CustomTokenOption>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
            services.AddScoped<ISupportTicketService, SupportTicketService>();
            services.AddScoped<IAutomaticPaymentRepository, AutomaticPaymentRepository>();
            services.AddScoped<IAutomaticPaymentService, AutomaticPaymentService>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IHttpContextData, HttpContextData>();
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AccountCreateDTOValidator>());

            services.Configure<CustomTokenOption>(configuration.GetSection("TokenOption"));

            services.AddDbContext<FinTechDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("SqlConnection"), action =>
                {
                    action.MigrationsAssembly("FinTech.Infrastructure");
                });
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<FinTechDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                var tokenOptions = configuration.GetSection("TokenOption").Get<CustomTokenOption>();
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("TokenOption:SecurityKey").Value)),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobkey = new JobKey("automaticPayment");

                q.AddJob<AutomaticPaymentJob>(opt => opt.WithIdentity(jobkey));

                q.AddTrigger(opt =>
                opt.ForJob(jobkey)
                .WithIdentity("automaticPayment-trigger")
                .WithCronSchedule("0 0/1 * * * ?"));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
