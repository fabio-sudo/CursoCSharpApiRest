using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplicationAPI.Context;
using WebApplicationAPI.Interfaces;
using WebApplicationAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Pega a connection string do appsettings.json
string connectionString = builder.Configuration.GetConnectionString("strConexao")
    ?? throw new InvalidOperationException("Connection string 'strConexao' não encontrada.");

// Registra o DbContext para injeção de dependência usando SQL Server
builder.Services.AddDbContext<WebApiContext>(options =>
    options.UseSqlServer(connectionString));

// Registra o repositório
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Registra o serviço para gerar token
builder.Services.AddScoped<TokenService>();

// Configura autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // ou true, dependendo do seu cenário
        ValidateAudience = false, // ou true, dependendo do seu cenário
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();

// Configura Swagger com autenticação JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplicationAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,     // aqui mudou para Http
        Scheme = "bearer",                   // e aqui para "bearer" (minúsculo)
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // IMPORTANTÍSSIMO ativar autenticação antes de autorização
app.UseAuthorization();

app.MapControllers();

app.Run();
