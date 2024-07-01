/// <summary>
/// Представляет отель с основной информацией, такой как ID, имя, широта и долгота.
/// </summary>
public class Hotel
{
    /// <summary>
    /// Получает или задает уникальный идентификатор отеля.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает имя отеля. По умолчанию это пустая строка.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает широту местоположения отеля.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Получает или задает долготу местоположения отеля.
    /// </summary>
    public double Longitude { get; set; }
}
