/// <summary>
/// Интерфейс для репозитория отелей, включающий методы для выполнения асинхронных операций CRUD и управления ресурсами.
/// </summary>
public interface IHotelRepository : IDisposable
{
    /// <summary>
    /// Асинхронно получает список всех отелей.
    /// </summary>
    /// <returns>Список отелей.</returns>
    Task<List<Hotel>> GetHotelsAsync();

    /// <summary>
    /// Асинхронно получает список отелей по имени.
    /// </summary>
    /// <param name="name">Имя отеля для поиска.</param>
    /// <returns>Список отелей, соответствующих указанному имени.</returns>
    Task<List<Hotel>> GetHotelsAsync(string name);

    /// <summary>
    /// Асинхронно получает список отелей по координатам.
    /// </summary>
    /// <param name="coordinate">Координаты для поиска отелей.</param>
    /// <returns>Список отелей, соответствующих указанным координатам.</returns>
    Task<List<Hotel>> GetHotelsAsync(Coordinate coordinate);

    /// <summary>
    /// Асинхронно получает информацию об отеле по его идентификатору.
    /// </summary>
    /// <param name="hotelId">Идентификатор отеля.</param>
    /// <returns>Отель, соответствующий указанному идентификатору.</returns>
    Task<Hotel> GetHotelAsync(int hotelId);

    /// <summary>
    /// Асинхронно вставляет новый отель в базу данных.
    /// </summary>
    /// <param name="hotel">Отель для вставки.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task InsertHotelAsync(Hotel hotel);

    /// <summary>
    /// Асинхронно обновляет информацию об отеле в базе данных.
    /// </summary>
    /// <param name="hotel">Отель для обновления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task UpdateHotelAsync(Hotel hotel);

    /// <summary>
    /// Асинхронно удаляет отель из базы данных по его идентификатору.
    /// </summary>
    /// <param name="hotelId">Идентификатор отеля для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task DeleteHotelAsync(int hotelId);

    /// <summary>
    /// Асинхронно сохраняет все изменения в базе данных.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task SaveAsync();
}
