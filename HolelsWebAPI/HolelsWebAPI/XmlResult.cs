/// <summary>
/// Представляет результат выполнения, который сериализует объект в XML и записывает его в тело HTTP-ответа.
/// </summary>
/// <typeparam name="T">Тип объекта, который будет сериализован в XML.</typeparam>
public class XmlResult<T> : IResult
{
    // Сериализатор XML для типа T.
    private static readonly XmlSerializer _xmlSerializer = new(typeof(T));
    // Результат, который будет сериализован.
    private readonly T _result;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="XmlResult{T}"/> с указанным результатом.
    /// </summary>
    /// <param name="result">Объект, который будет сериализован в XML.</param>
    public XmlResult(T result) => _result = result;

    /// <summary>
    /// Выполняет асинхронное выполнение, сериализуя объект в XML и записывая его в тело HTTP-ответа.
    /// </summary>
    /// <param name="httpContext">Контекст HTTP-запроса.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public Task ExecuteAsync(HttpContext httpContext)
    {
        // Создаем поток в памяти для сериализации.
        using var ms = new MemoryStream();
        // Сериализуем объект в поток.
        _xmlSerializer.Serialize(ms, _result);
        // Устанавливаем позицию потока в начало.
        ms.Position = 0;
        // Копируем содержимое потока в тело ответа.
        return ms.CopyToAsync(httpContext.Response.Body);
    }
}

/// <summary>
/// Предоставляет метод расширения для создания XML-результата.
/// </summary>
static class XmlResultExtentions
{
    /// <summary>
    /// Создает новый XML-результат с указанным объектом.
    /// </summary>
    /// <typeparam name="T">Тип объекта, который будет сериализован в XML.</typeparam>
    /// <param name="_">Параметр расширения, который не используется.</param>
    /// <param name="result">Объект, который будет сериализован в XML.</param>
    /// <returns>XML-результат.</returns>
    public static IResult Xml<T>(this IResultExtensions _, T result) =>
        new XmlResult<T>(result);
}
