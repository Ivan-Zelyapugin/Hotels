/// <summary>
/// Интерфейс для репозитория пользователей, включающий метод для получения данных пользователя на основе модели пользователя.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получает объект передачи данных пользователя (DTO) на основе предоставленной модели пользователя.
    /// </summary>
    /// <param name="userModel">Модель данных пользователя.</param>
    /// <returns>Объект передачи данных пользователя (DTO).</returns>
    UserDto GetUser(UserModel userModel);
}
