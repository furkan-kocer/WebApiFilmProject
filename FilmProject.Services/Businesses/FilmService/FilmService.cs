using FilmProject.Contracts;
using FilmProject.Contracts.Abstractions.Businesses.FilmServiceAbs;
using FilmProject.Contracts.DataTransferObjects.Film;
using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
using FilmProject.DataAccess.Entities;
using FilmProject.Services.Helpers;
using Mapster;

namespace FilmProject.Services.Businesses.FilmService
{
    public class FilmService : IFilmService
    {
        private readonly IFilmCollectionRepository _filmCollectionRepository;
        public FilmService(IFilmCollectionRepository filmCollectionService)
        {
            _filmCollectionRepository = filmCollectionService;
        }

        public async Task<GenericResponseBase<string>> CreateFilm(FilmRequest filmRequest)
        {
            var film = filmRequest.Adapt<Film>();
            film.updatedDate = DateTime.UtcNow;
            film.createdDate = DateTime.UtcNow;
            film.status = true;
            film.FilmCode = HelperService.GenerateUniqueCode(filmRequest.FilmName);
            await _filmCollectionRepository.CreateFilmAsync(film);
            return GenericResponseBase<string>.Success();
        }
        public async Task<GenericResponseBase<string>> UpdateFilmByCode(FilmRequest filmRequest, string filmCode)
        {
            var film = filmRequest.Adapt<Film>();
            film.updatedDate = DateTime.UtcNow;
            await _filmCollectionRepository.UpdateOneFilmByCode(film, filmCode);
            return GenericResponseBase<string>.Success();
        }
        public async Task DeleteAllFilms()
        {
            await _filmCollectionRepository.DeleteAllFilmsAsync();
        }

        public async Task DeleteSpecificFilmAsync(string filmCode)
        {
            await _filmCollectionRepository.DeleteFilmAsync(filmCode);
        }

        public async Task<GenericResponseBase<List<FilmResponse>>> GetAllFilmsAsync()
        {
            var films = await _filmCollectionRepository.GetFilmsAsync();
            var filmResponses = films.Adapt<List<FilmResponse>>();
            return GenericResponseBase<List<FilmResponse>>.Success(filmResponses);
        }

        public async Task<GenericResponseBase<FilmResponse>> GetSpecificFilmAsync(string filmCode)
        {
            var film = await _filmCollectionRepository.GetSpecificFilm(filmCode);
            if (film == null)
            {
                return GenericResponseBase<FilmResponse>.NotFound("There is no film founded with value code:" + filmCode);
            }
            var filmResponse = film.Adapt<FilmResponse>();
            return GenericResponseBase<FilmResponse>.Success(filmResponse);
        }
    }
}
