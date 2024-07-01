/// <summary>
/// Репозиторий пользователей, реализующий интерфейс IUserRepository.
/// Предоставляет метод для получения данных пользователя на основе модели пользователя.
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// Список пользователей.
    /// </summary>
    private List<UserDto> _users => new()
    {
        new UserDto("John", "123"),
        new UserDto("Monica", "123"),
        new UserDto("Nancy", "123")
    };

    /// <summary>
    /// Получает объект передачи данных пользователя (DTO) на основе предоставленной модели пользователя.
    /// </summary>
    /// <param name="userModel">Модель данных пользователя.</param>
    /// <returns>Объект передачи данных пользователя (DTO).</returns>
    public UserDto GetUser(UserModel userModel) =>
        _users.FirstOrDefault(u =>
            string.Equals(u.UserName, userModel.UserName) &&
            string.Equals(u.Password, userModel.Password)) ??
            throw new Exception("Пользователь не найден или неверный пароль.");
}
