using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Класс для регистрации маршрутов управления данными отелей в веб-приложении ASP.NET Core.
/// </summary>
namespace HolelsWebAPI.Apis
{
    public class HotelApi : IApi
    {
        /// <summary>
        /// Регистрирует маршруты управления данными отелей в указанном приложении.
        /// </summary>
        /// <param name="app">Веб-приложение ASP.NET Core, в котором будут зарегистрированы маршруты.</param>
        public void Register(WebApplication app)
        {
            app.MapGet("/hotels", Get)
                .Produces<List<Hotel>>(StatusCodes.Status200OK)
                .WithName("GetAllHotels")
                .WithTags("Getters");

            app.MapGet("/hotels/{id}", GetById)
                .Produces<Hotel>(StatusCodes.Status200OK)
                .WithName("GetHotel")
                .WithTags("Getters");

            app.MapPost("/hotels", Post)
                .Accepts<Hotel>("application/json")
                .Produces<Hotel>(StatusCodes.Status201Created)
                .WithName("CreateHotel")
                .WithTags("Creators");

            app.MapPut("/hotels", Put)
                .Accepts<Hotel>("application/json")
                .WithName("UpdateHotel")
                .WithTags("Updaters");

            app.MapDelete("hotels/{id}", Delete)
                .WithName("DeleteHotel")
                .WithTags("Deleters");

            app.MapGet("/hotels/search/name/{query}", SearchByName)
                .Produces<List<Hotel>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("SearchHotels")
                .WithTags("Getters")
                .ExcludeFromDescription();

            app.MapGet("/hotels/search/location/{coordinate}", SearchByCoordinate)
                .ExcludeFromDescription();
        }

        /// <summary>
        /// Получает список всех отелей.
        /// </summary>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат в формате XML.</returns>
        [Authorize]
        private async Task<IResult> Get(IHotelRepository repository) =>
            Results.Extensions.Xml(await repository.GetHotelsAsync());

        /// <summary>
        /// Получает отель по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор отеля.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат OK или NotFound.</returns>
        [Authorize]
        private async Task<IResult> GetById(int id, IHotelRepository repository) =>
            await repository.GetHotelAsync(id) is Hotel hotel
             ? Results.Ok(hotel)
             : Results.NotFound();

        /// <summary>
        /// Создает новый отель.
        /// </summary>
        /// <param name="hotel">Модель отеля.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат Created.</returns>
        [Authorize]
        private async Task<IResult> Post([FromBody] Hotel hotel, IHotelRepository repository)
        {
            await repository.InsertHotelAsync(hotel);
            await repository.SaveAsync();
            return Results.Created($"/hotels/{hotel.Id}", hotel);
        }

        /// <summary>
        /// Обновляет существующий отель.
        /// </summary>
        /// <param name="hotel">Модель отеля.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат NoContent.</returns>
        [Authorize]
        private async Task<IResult> Put([FromBody] Hotel hotel, IHotelRepository repository)
        {
            await repository.UpdateHotelAsync(hotel);
            await repository.SaveAsync();
            return Results.NoContent();
        }

        /// <summary>
        /// Удаляет отель по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор отеля.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат NoContent.</returns>
        [Authorize]
        private async Task<IResult> Delete(int id, IHotelRepository repository)
        {
            await repository.DeleteHotelAsync(id);
            await repository.SaveAsync();
            return Results.NoContent();
        }

        /// <summary>
        /// Ищет отели по имени.
        /// </summary>
        /// <param name="query">Запрос поиска.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат OK или NotFound.</returns>
        [Authorize]
        private async Task<IResult> SearchByName(string query, IHotelRepository repository) =>
             await repository.GetHotelsAsync(query) is IEnumerable<Hotel> hotels
                 ? Results.Ok(hotels)
                 : Results.NotFound(Array.Empty<Hotel>());

        /// <summary>
        /// Ищет отели по координатам.
        /// </summary>
        /// <param name="coordinate">Координаты поиска.</param>
        /// <param name="repository">Репозиторий отелей.</param>
        /// <returns>Результат OK или NotFound.</returns>
        [Authorize]
        private async Task<IResult> SearchByCoordinate(Coordinate coordinate, IHotelRepository repository) =>
              await repository.GetHotelsAsync(coordinate) is IEnumerable<Hotel> hotels
                  ? Results.Ok(hotels)
                  : Results.NotFound(Array.Empty<Hotel>());
    }
}
