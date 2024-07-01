/// <summary>
/// Репозиторий отелей, реализующий интерфейс IHotelRepository и предоставляющий методы для выполнения асинхронных операций CRUD.
/// </summary>
public class HotelRepository : IHotelRepository
{
    private readonly HotelDb _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="HotelRepository"/> с указанным контекстом базы данных.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public HotelRepository(HotelDb context)
    {
        _context = context;
    }

    /// <summary>
    /// Асинхронно получает список всех отелей.
    /// </summary>
    /// <returns>Список отелей.</returns>
    public Task<List<Hotel>> GetHotelsAsync() => _context.Hotels.ToListAsync();

    /// <summary>
    /// Асинхронно получает список отелей по имени.
    /// </summary>
    /// <param name="name">Имя отеля для поиска.</param>
    /// <returns>Список отелей, соответствующих указанному имени.</returns>
    public Task<List<Hotel>> GetHotelsAsync(string name) =>
        _context.Hotels.Where(h => h.Name.Contains(name)).ToListAsync();

    /// <summary>
    /// Асинхронно получает список отелей по координатам.
    /// </summary>
    /// <param name="coordinate">Координаты для поиска отелей.</param>
    /// <returns>Список отелей, соответствующих указанным координатам.</returns>
    public async Task<List<Hotel>> GetHotelsAsync(Coordinate coordinate) =>
        await _context.Hotels.Where(hotel =>
            hotel.Latitude > coordinate.Latilude - 1 &&
            hotel.Latitude < coordinate.Latilude + 1 &&
            hotel.Longitude > coordinate.Longitude - 1 &&
            hotel.Longitude < coordinate.Longitude + 1
        ).ToListAsync();

    /// <summary>
    /// Асинхронно получает информацию об отеле по его идентификатору.
    /// </summary>
    /// <param name="hotelId">Идентификатор отеля.</param>
    /// <returns>Отель, соответствующий указанному идентификатору.</returns>
    public async Task<Hotel> GetHotelAsync(int hotelId) =>
        await _context.Hotels.FindAsync(new object[] { hotelId });

    /// <summary>
    /// Асинхронно вставляет новый отель в базу данных.
    /// </summary>
    /// <param name="hotel">Отель для вставки.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task InsertHotelAsync(Hotel hotel) => await _context.Hotels.AddAsync(hotel);

    /// <summary>
    /// Асинхронно обновляет информацию об отеле в базе данных.
    /// </summary>
    /// <param name="hotel">Отель для обновления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task UpdateHotelAsync(Hotel hotel)
    {
        var hotelFromDb = await _context.Hotels.FindAsync(new object[] { hotel.Id });
        if (hotelFromDb == null) return;
        hotelFromDb.Longitude = hotel.Longitude;
        hotelFromDb.Latitude = hotel.Latitude;
        hotelFromDb.Name = hotel.Name;
    }

    /// <summary>
    /// Асинхронно удаляет отель из базы данных по его идентификатору.
    /// </summary>
    /// <param name="hotelId">Идентификатор отеля для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task DeleteHotelAsync(int hotelId)
    {
        var hotelFromDb = await _context.Hotels.FindAsync(new object[] { hotelId });
        if (hotelFromDb == null) return;
        _context.Hotels.Remove(hotelFromDb);
    }

    /// <summary>
    /// Асинхронно сохраняет все изменения в базе данных.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task SaveAsync() => await _context.SaveChangesAsync();

    private bool _disposed = false;

    /// <summary>
    /// Освобождает управляемые ресурсы.
    /// </summary>
    /// <param name="disposing">Указывает, следует ли освобождать управляемые ресурсы.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    /// <summary>
    /// Освобождает ресурсы, управляемые реализацией HotelRepository.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
