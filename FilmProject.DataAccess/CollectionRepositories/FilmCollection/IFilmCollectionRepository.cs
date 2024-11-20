using FilmProject.DataAccess.Entities;

namespace FilmProject.DataAccess.CollectionRepositories.FilmCollection
{
    public interface IFilmCollectionRepository
    {
        Task CreateFilmAsync(Film film);
        Task<List<Film>> GetFilmsAsync();
        Task<Film> GetSpecificFilm(string filmCode);
        Task UpdateOneFilmByCode(Film film, string filmCode);
        Task DeleteFilmAsync(string filmCode);
        Task DeleteAllFilmsAsync();
    }
}
