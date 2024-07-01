/// <summary>
/// Интерфейс для регистрации API в веб-приложении ASP.NET Core.
/// </summary>
namespace HolelsWebAPI.Apis
{
    public interface IApi
    {
        /// <summary>
        /// Регистрирует API в указанном приложении.
        /// </summary>
        /// <param name="app">Веб-приложение ASP.NET Core, в котором будет зарегистрирован API.</param>
        void Register(WebApplication app);
    }
}
