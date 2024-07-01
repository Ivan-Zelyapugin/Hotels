/// <summary>
/// Представляет объект передачи данных пользователя (DTO) с именем пользователя и паролем.
/// </summary>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Password">Пароль пользователя.</param>
public record UserDto(string UserName, string Password);

/// <summary>
/// Представляет модель данных пользователя с именем пользователя и паролем, включая атрибуты валидации.
/// </summary>
public record UserModel
{
    /// <summary>
    /// Получает или задает имя пользователя. Поле обязательно для заполнения.
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает пароль пользователя. Поле обязательно для заполнения.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
