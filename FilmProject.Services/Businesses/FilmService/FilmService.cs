using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
using FilmProject.DataAccess.DataTransferObjects.Film;
using FilmProject.DataAccess.Entities;
using FilmProject.DataAccess.Helpers;

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
            try
            {
                var film = new Film
                {
                    filmName = filmRequest.filmName,
                    price = filmRequest.price,
                    status = "yayında",
                    createdDate = DateTime.Now,
                    updatedDate = DateTime.Now,
                    filmCode = HelperService.GenerateUniqueCode(filmRequest.filmName)
                };
                await _filmCollectionRepository.CreateFilmAsync(film);
                return GenericResponseBase<string>.Success();
            }
            catch (Exception ex)
            {
                return GenericResponseBase<string>.Error(ex.Message);
            }
        }
        public async Task<GenericResponseBase<string>> UpdateFilmByCode(FilmRequest filmRequest, string filmCode)
        {
            try
            {
                var film = new Film
                {
                    filmName = filmRequest.filmName,
                    price = filmRequest.price,
                    updatedDate = DateTime.Now
                };
                await _filmCollectionRepository.UpdateOneFilmByCode(film, filmCode);
                return GenericResponseBase<string>.Success();
            }
            catch (Exception ex)
            {
                return GenericResponseBase<string>.Error(ex.Message);
            }
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
            try
            {
                var films = await _filmCollectionRepository.GetFilmsAsync();
                //if (response == null || !response.Any())
                //{
                //    //return GenericResponseBase<List<FilmResponse>>.NotFound("No films found.");
                //}
                var filmResponses = new List<FilmResponse>();
                foreach (var film in films)
                {
                    var filmResponse = new FilmResponse(filmName: film.filmName, price: film.price, filmCode: film.filmCode);
                    filmResponses.Add(filmResponse);
                }
                return GenericResponseBase<List<FilmResponse>>.Success(filmResponses);
            }
            catch (Exception ex)
            {
                return GenericResponseBase<List<FilmResponse>>.Error(ex.Message);
            }
        }

        public async Task<GenericResponseBase<FilmResponse>> GetSpecificFilmAsync(string filmCode)
        {
            try
            {
                var film = await _filmCollectionRepository.GetSpecificFilm(filmCode);
                if(film == null)
                {
                    return GenericResponseBase<FilmResponse>.NotFound("There is no film founded with " + filmCode + " code value.");
                }
                var filmResponse = new FilmResponse(filmName: film.filmName, price:film.price, filmCode: film.filmCode);
                return GenericResponseBase<FilmResponse>.Success(filmResponse);
            }
            catch (Exception ex)
            {
                return GenericResponseBase<FilmResponse>.Error(ex.Message);
            }
        }
    }
}
