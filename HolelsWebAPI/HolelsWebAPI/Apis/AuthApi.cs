/// <summary>
/// Класс для регистрации маршрутов аутентификации в веб-приложении ASP.NET Core.
/// </summary>
namespace HolelsWebAPI.Apis
{
    public class AuthApi : IApi
    {
        /// <summary>
        /// Регистрирует маршруты аутентификации в указанном приложении.
        /// </summary>
        /// <param name="app">Веб-приложение ASP.NET Core, в котором будут зарегистрированы маршруты.</param>
        public void Register(WebApplication app)
        {
            app.MapGet("/login", [AllowAnonymous] async (HttpContext context,
                ITokenService tokenService, IUserRepository userRepository) =>
            {
                // Создаем модель пользователя из параметров запроса
                UserModel userModel = new()
                {
                    UserName = context.Request.Query["username"],
                    Password = context.Request.Query["password"]
                };

                // Получаем данные пользователя
                var userDto = userRepository.GetUser(userModel);

                // Если пользователь не найден, возвращаем Unauthorized
                if (userDto == null) return Results.Unauthorized();

                // Строим JWT токен
                var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"], userDto);

                // Возвращаем токен в ответе
                return Results.Ok(token);
            });
        }
    }
}
