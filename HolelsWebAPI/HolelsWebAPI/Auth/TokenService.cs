/// <summary>
/// Сервис для создания JWT токенов, реализующий интерфейс ITokenService.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Продолжительность срока действия токена. (30 минут)
    /// </summary>
    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

    /// <summary>
    /// Создает JWT токен на основе предоставленного ключа, издателя и данных пользователя.
    /// </summary>
    /// <param name="key">Ключ для подписи токена.</param>
    /// <param name="issuer">Издатель токена.</param>
    /// <param name="user">Данные пользователя для включения в токен.</param>
    /// <returns>Строка, представляющая созданный токен.</returns>
    public string BuildToken(string key, string issuer, UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.Add(ExpiryDuration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
