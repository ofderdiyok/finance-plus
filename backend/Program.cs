using FinancePlus.Data;
using FinancePlus.Services;
using FinancePlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:80");

// --- Configuration ---
builder.Services.AddControllers(); // For API controllers
builder.Services.AddEndpointsApiExplorer(); // For Swagger
builder.Services.AddSwaggerGen();

// --- Database ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Services ---
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// --- Middleware ---
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers(); // Enable API controller routing

// --- Test endpoint ---
app.MapGet("/test", () => "FinancePlus API çalışıyor ✅");

// --- Run ---
app.Run();
