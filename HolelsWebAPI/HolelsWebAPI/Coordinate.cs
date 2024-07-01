/// <summary>
/// Представляет координаты с широтой и долготой.
/// </summary>
/// <param name="Latilude">Широта.</param>
/// <param name="Longitude">Долгота.</param>
public record Coordinate(double Latilude, double Longitude)
{
    /// <summary>
    /// Пытается разобрать строку в координаты.
    /// </summary>
    /// <param name="input">Строка, содержащая координаты в формате "широта,долгота".</param>
    /// <param name="coordinate">Разобранные координаты.</param>
    /// <returns>True, если парсинг успешен; иначе False.</returns>
    public static bool TryParse(string input, out Coordinate? coordinate)
    {
        coordinate = default;
        var splitArray = input.Split(',', 2);
        if (splitArray.Length != 2) return false;
        if (!double.TryParse(splitArray[0], out var lat)) return false;
        if (!double.TryParse(splitArray[1], out var lon)) return false;
        coordinate = new(lat, lon);
        return true;
    }

    /// <summary>
    /// Асинхронно связывает строку координат с контекстом HTTP.
    /// </summary>
    /// <param name="context">Контекст HTTP.</param>
    /// <param name="parameter">Информация о параметре.</param>
    /// <returns>Координаты, если парсинг успешен; иначе null.</returns>
    public static async ValueTask<Coordinate?> BindAsync(HttpContext context,
        ParameterInfo parameter)
    {
        var input = context.GetRouteValue(parameter.Name!) as string ?? string.Empty;
        TryParse(input, out var coordinate);
        return coordinate;
    }
}
