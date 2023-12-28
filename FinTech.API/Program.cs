using FinTech.Core.Interfaces;
using FinTech.Core.Mapper;
using FinTech.Infrastructure;
using FinTech.Infrastructure.Database;
using FinTech.Infrastructure.Repositories;
using FinTech.Service.Interfaces;
using FinTech.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAplication();
builder.Services.AddDbContext<FinTechDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SqlConnection"), Action => {
        Action.MigrationsAssembly("FinTech.Infrastructure");
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
