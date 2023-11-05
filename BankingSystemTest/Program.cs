using BankingSystemTest.Database;
using BankingSystemTest.User;
using BankingSystemTest.UserAccount;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankingDatabase>(opt => opt.UseInMemoryDatabase("Bankingsystem"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.RegisterUserEndpoints();
app.RegisterUserAccountEndpoints();
app.Run();

