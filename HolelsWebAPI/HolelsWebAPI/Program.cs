global using Microsoft.EntityFrameworkCore;
global using System.Xml.Serialization;
global using System.Reflection;
global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
using HolelsWebAPI.Apis;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services); // Вызов метода для регистрации необходимых сервисов.

var app = builder.Build(); 

Configure(app); // Вызов метода для конфигурации приложения.

var apis = app.Services.GetServices<IApi>(); // Получение всех зарегистрированных сервисов, реализующих интерфейс IApi.
foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found"); 
    api.Register(app); // Регистрация каждого API в приложении.
}

app.Run(); 

/// <summary>
/// Регистрирует необходимые сервисы в контейнере зависимостей.
/// </summary>
/// <param name="services">Коллекция сервисов для регистрации.</param>
void RegisterServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer(); // Добавление поддержки для генерации документации конечных точек API.
    services.AddSwaggerGen(); // Добавление поддержки для генерации Swagger документации.
    services.AddDbContext<HotelDb>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")); // Конфигурация контекста базы данных для использования SQLite.
    });
    services.AddScoped<IHotelRepository, HotelRepository>(); // Регистрация HotelRepository как реализации IHotelRepository с областью действия "scoped".
    services.AddSingleton<ITokenService>(new TokenService()); // Регистрация TokenService как singleton.
    services.AddSingleton<IUserRepository>(new UserRepository()); // Регистрация UserRepository как singleton.
    services.AddAuthorization(); // Добавление поддержки авторизации.
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true, // Валидация издателя токена.
                ValidateAudience = true, // Валидация аудитории токена.
                ValidateLifetime = true, // Валидация времени жизни токена.
                ValidateIssuerSigningKey = true, // Валидация ключа подписи издателя.
                ValidIssuer = builder.Configuration["Jwt:Issuer"], // Настройка допустимого издателя.
                ValidAudience = builder.Configuration["Jwt:Audience"], // Настройка допустимой аудитории.
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Настройка ключа подписи токена.
            };
        });

    services.AddTransient<IApi, HotelApi>(); // Регистрация HotelApi как реализации IApi с областью действия "transient".
    services.AddTransient<IApi, AuthApi>(); // Регистрация AuthApi как реализации IApi с областью действия "transient".
}

/// <summary>
/// Конфигурирует приложение.
/// </summary>
/// <param name="app">Веб-приложение ASP.NET Core.</param>
void Configure(WebApplication app)
{
    app.UseAuthentication(); // Включение аутентификации.
    app.UseAuthorization(); // Включение авторизации.

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); 
        app.UseSwaggerUI(); 
        using var scope = app.Services.CreateScope(); // Создание области для получения сервисов.
        var db = scope.ServiceProvider.GetRequiredService<HotelDb>(); // Получение сервиса HotelDb.
        db.Database.EnsureCreated(); // Обеспечение создания базы данных при запуске приложения.
    }

    app.UseHttpsRedirection(); // Принудительное перенаправление HTTP-запросов на HTTPS.
}
