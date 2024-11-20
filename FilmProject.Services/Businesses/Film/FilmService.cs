using FilmProject.DataAccess.DataTransferObjects.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.Services.Businesses.Film
{
    public class FilmService : IFilmService
    {
        public Task<GenericResponseBase<string>> CreateFilm(FilmRequest filmRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllFilms()
        {
            throw new NotImplementedException();
        }

        public Task DeleteSpecificFilmAsync(string filmCode)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseBase<List<FilmResponse>>> GetAllFilmsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseBase<FilmResponse>> GetSpecificFilmAsync(string filmCode)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseBase<string>> UpdateFilmByCode(FilmRequest filmRequest, string filmCode)
        {
            throw new NotImplementedException();
        }
    }
}
