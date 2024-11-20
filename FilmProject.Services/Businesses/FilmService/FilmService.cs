﻿using FilmProject.DataAccess.CollectionRepositories.FilmCollection;
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
        public async Task<GenericResponseBase<string>> UpdateFilmByCode(FilmRequest filmRequest, string filmCode)
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
            var filmResponses = new List<FilmResponse>();
            foreach (var film in films)
            {
                var filmResponse = new FilmResponse(filmName: film.filmName, price: film.price, filmCode: film.filmCode);
                filmResponses.Add(filmResponse);
            }
            return GenericResponseBase<List<FilmResponse>>.Success(filmResponses);
        }

        public async Task<GenericResponseBase<FilmResponse>> GetSpecificFilmAsync(string filmCode)
        {
            var film = await _filmCollectionRepository.GetSpecificFilm(filmCode);
            if (film == null)
            {
                return GenericResponseBase<FilmResponse>.NotFound("There is no film founded with " + filmCode + " code value.");
            }
            var filmResponse = new FilmResponse(filmName: film.filmName, price: film.price, filmCode: film.filmCode);
            return GenericResponseBase<FilmResponse>.Success(filmResponse);
        }
    }
}
