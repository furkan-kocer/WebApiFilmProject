using FilmProject.DataAccess.DataTransferObjects.Film;

namespace FilmProject.Services.Businesses.FilmService
{
    public interface IFilmService
    {
        Task<GenericResponseBase<string>> CreateFilm(FilmRequest filmRequest);
        Task<GenericResponseBase<List<FilmResponse>>> GetAllFilmsAsync();
        Task<GenericResponseBase<FilmResponse>> GetSpecificFilmAsync(string filmCode);
        Task<GenericResponseBase<string>> UpdateFilmByCode(FilmRequest filmRequest, string filmCode);
        Task DeleteSpecificFilmAsync(string filmCode);
        Task DeleteAllFilms();
    }
}
