/// <summary>
/// Интерфейс для сервиса токенов, включающий метод для создания токена на основе ключа, издателя и данных пользователя.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Создает токен на основе предоставленного ключа, издателя и данных пользователя.
    /// </summary>
    /// <param name="key">Ключ для подписи токена.</param>
    /// <param name="issuer">Издатель токена.</param>
    /// <param name="user">Данные пользователя для включения в токен.</param>
    /// <returns>Строка, представляющая созданный токен.</returns>
    string BuildToken(string key, string issuer, UserDto user);
}
