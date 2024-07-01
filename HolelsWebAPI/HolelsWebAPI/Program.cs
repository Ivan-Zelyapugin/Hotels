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

RegisterServices(builder.Services); // ����� ������ ��� ����������� ����������� ��������.

var app = builder.Build(); 

Configure(app); // ����� ������ ��� ������������ ����������.

var apis = app.Services.GetServices<IApi>(); // ��������� ���� ������������������ ��������, ����������� ��������� IApi.
foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found"); 
    api.Register(app); // ����������� ������� API � ����������.
}

app.Run(); 

/// <summary>
/// ������������ ����������� ������� � ���������� ������������.
/// </summary>
/// <param name="services">��������� �������� ��� �����������.</param>
void RegisterServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer(); // ���������� ��������� ��� ��������� ������������ �������� ����� API.
    services.AddSwaggerGen(); // ���������� ��������� ��� ��������� Swagger ������������.
    services.AddDbContext<HotelDb>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")); // ������������ ��������� ���� ������ ��� ������������� SQLite.
    });
    services.AddScoped<IHotelRepository, HotelRepository>(); // ����������� HotelRepository ��� ���������� IHotelRepository � �������� �������� "scoped".
    services.AddSingleton<ITokenService>(new TokenService()); // ����������� TokenService ��� singleton.
    services.AddSingleton<IUserRepository>(new UserRepository()); // ����������� UserRepository ��� singleton.
    services.AddAuthorization(); // ���������� ��������� �����������.
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true, // ��������� �������� ������.
                ValidateAudience = true, // ��������� ��������� ������.
                ValidateLifetime = true, // ��������� ������� ����� ������.
                ValidateIssuerSigningKey = true, // ��������� ����� ������� ��������.
                ValidIssuer = builder.Configuration["Jwt:Issuer"], // ��������� ����������� ��������.
                ValidAudience = builder.Configuration["Jwt:Audience"], // ��������� ���������� ���������.
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // ��������� ����� ������� ������.
            };
        });

    services.AddTransient<IApi, HotelApi>(); // ����������� HotelApi ��� ���������� IApi � �������� �������� "transient".
    services.AddTransient<IApi, AuthApi>(); // ����������� AuthApi ��� ���������� IApi � �������� �������� "transient".
}

/// <summary>
/// ������������� ����������.
/// </summary>
/// <param name="app">���-���������� ASP.NET Core.</param>
void Configure(WebApplication app)
{
    app.UseAuthentication(); // ��������� ��������������.
    app.UseAuthorization(); // ��������� �����������.

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); 
        app.UseSwaggerUI(); 
        using var scope = app.Services.CreateScope(); // �������� ������� ��� ��������� ��������.
        var db = scope.ServiceProvider.GetRequiredService<HotelDb>(); // ��������� ������� HotelDb.
        db.Database.EnsureCreated(); // ����������� �������� ���� ������ ��� ������� ����������.
    }

    app.UseHttpsRedirection(); // �������������� ��������������� HTTP-�������� �� HTTPS.
}
