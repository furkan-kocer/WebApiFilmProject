using FilmProject.DataAccess.DataTransferObjects.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.Services.Businesses.Film
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
