using Microsoft.EntityFrameworkCore;
using SISTEMARH_BACKEND.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte ao Swagger (documentação interativa da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o acesso ao banco PostgreSQL via Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona suporte a controllers e configura JSON para evitar loops de referência
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Configuração da autenticação JWT (Bearer Token)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Não valida o emissor do token
            ValidateAudience = false, // Não valida o público-alvo do token
            ValidateLifetime = true, // Valida o tempo de expiração do token
            ValidateIssuerSigningKey = true, // Valida a chave usada na assinatura
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("chave-super-secreta-muito-forte-1234567890")) // Chave usada pra validar o token
        };
    });

var app = builder.Build();

// Ativa Swagger apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware de autenticação e autorização
app.UseAuthorization();   // Verifica se o usuário pode acessar a rota
app.UseAuthentication();  // Lê o token JWT da requisição e valida

// Mapeia todos os controllers da aplicação
app.MapControllers();

// Inicia a aplicação
app.Run();
