using FinTech.Core.Entities;
using FinTech.Core.Constans;
using FinTech.Core.Interfaces;
using FinTech.Core.Mapper;
using FinTech.Core.Models;
using FinTech.Infrastructure;
using FinTech.Infrastructure.Database;
using FinTech.Infrastructure.Repositories;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jwtToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }
});
} );
builder.Services.AddAplication(builder.Configuration);
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinTechDbContext>();
    if (!dbContext.Roles.Any())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await roleManager.CreateAsync(new() { Name = "admin"});
        await roleManager.CreateAsync(new() { Name = "manager"});
        await roleManager.CreateAsync(new() { Name = "loanofficer" });
        await roleManager.CreateAsync(new() { Name = "supportticketanalyst" });
        await roleManager.CreateAsync(new() { Name = "customersupport" });
        await roleManager.CreateAsync(new() { Name = "customer" });
    }
    if (!dbContext.Users.Any())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = new ApplicationUser() { UserName = "admin" , Name = "Admin" , Surname = "AdminSurname" , IdentityNumber = "11111111111" , Address = ""};
        await userManager.CreateAsync(admin, "Asd123*");
        await userManager.AddToRoleAsync(admin, "admin");

        var manager = new ApplicationUser() { UserName = "manager", Name = "Manager", Surname = "ManagerSurname", IdentityNumber = "11111111112", Address = "" };
        await userManager.CreateAsync(manager, "Asd123*");
        await userManager.AddToRoleAsync(manager, "manager");

        var loanOfficer = new ApplicationUser() { UserName = "loanOfficer", Name = "loanOfficer", Surname = "loanOfficerSurname", IdentityNumber = "11111111113", Address = "" };
        await userManager.CreateAsync(loanOfficer, "Asd123*");
        await userManager.AddToRoleAsync(loanOfficer, "loanofficer");

        var customerSupport = new ApplicationUser() { UserName = "customerSupport", Name = "CustomerSupport", Surname = "CustomerSupportSurname", IdentityNumber = "11111111114", Address = ""};
        await userManager.CreateAsync(customerSupport, "Asd123*");
        await userManager.AddToRoleAsync(customerSupport, "supportticketanalyst");

        var supportTicketAnalyst = new ApplicationUser() { UserName = "supportTicketAnalyst", Name = "supportTicketAnalyst", Surname = "supportTicketAnalystSurname", IdentityNumber = "11111111115", Address = "" };
        await userManager.CreateAsync(supportTicketAnalyst, "Asd123*");
        await userManager.AddToRoleAsync(supportTicketAnalyst, "customersupport");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
