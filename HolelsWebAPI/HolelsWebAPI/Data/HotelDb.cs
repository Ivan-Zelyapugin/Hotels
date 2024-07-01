/// <summary>
/// Представляет контекст базы данных для управления сущностями отелей с использованием Entity Framework Core.
/// </summary>
public class HotelDb : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="HotelDb"/> с указанными параметрами.
    /// </summary>
    /// <param name="options">Параметры, используемые контекстом DbContext.</param>
    public HotelDb(DbContextOptions<HotelDb> options) : base(options) { }

    /// <summary>
    /// Получает набор сущностей отелей.
    /// </summary>
    public DbSet<Hotel> Hotels => Set<Hotel>();
}
