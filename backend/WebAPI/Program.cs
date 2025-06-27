using System.Text;
using Microsoft.EntityFrameworkCore;
using BLL.Services;
using BLL.Data;
using Core.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Core.Models.Audit;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore; // Для AddFluentValidation
using Audit.NET; // Для Audit
using Audit.EntityFramework; // Для AddInterceptors
using Microsoft.Extensions.DependencyInjection; // Для AddValidatorsFromAssemblyContaining

var builder = WebApplication.CreateBuilder(args);

// Добавляем DbContext
builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(new AuditSaveChangesInterceptor());
});

// Настройка Audit.NET (требует пакет Audit.NET)
Audit.Core.Configuration.Setup()
    .UseEntityFramework(_ => _
        .AuditTypeMapper(t => typeof(AuditLog))
        .AuditEntityAction<AuditLog>((ev, entry, entity) =>
        {
            entity.AuditData = entry.ToJson();
            entity.EntityType = entry.EntityType.Name;
            entity.UserName = ev.Environment.UserName;
            entity.AuditDate = DateTime.UtcNow;
        }));

// Регистрация FluentValidation (требует пакет FluentValidation.AspNetCore)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// Регистрация сервисов
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderTaskService>();

// JWT Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();

// Добавляем контроллеры
builder.Services.AddControllers();

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Аутентификация
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Политики авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireRole("admin"));
        
    options.AddPolicy("RequireUserRole", policy => 
        policy.RequireRole("admin", "user"));
});

var app = builder.Build();

// Настройка конвейера запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Настройка отлова глобальных ошибок
app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new 
        {
            error = exception?.Message ?? "An unexpected error occurred"
        }));
    });
});

// Автоматическое применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();