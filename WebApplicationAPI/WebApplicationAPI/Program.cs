using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Context;
using WebApplicationAPI.Interfaces;
using WebApplicationAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Pega a connection string do appsettings.json
string connectionString = builder.Configuration.GetConnectionString("strConexao")
    ?? throw new InvalidOperationException("Connection string 'strConexao' não encontrada.");


// Registra o DbContext para injeção de dependência usando SQL Server
builder.Services.AddDbContext<WebApiContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
