using FilmProject.DataAccess.Entities;
using MongoDB.Driver;

namespace FilmProject.DataAccess.CollectionRepositories.FilmCollection
{
    public class FilmCollectionRepository : IFilmCollectionRepository
    {
        private readonly MongoDBService _dbService;
        public FilmCollectionRepository(MongoDBService dbService)
        {
            _dbService = dbService;
        }
        public async Task CreateFilmAsync(Film film)
        {
            await _dbService._filmCollection.InsertOneAsync(film);
        }
        public async Task UpdateOneFilmByCode(Film film, string filmCode)
        {
            var filter = Builders<Film>.Filter.Eq(f => f.FilmCode, filmCode);
            var update = Builders<Film>.Update.Set(f => f.FilmName, film.FilmName)
                                              .Set(f => f.Price, film.Price)
                                              .Set(f => f.updatedDate, film.updatedDate);
            await _dbService._filmCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAllFilmsAsync()
        {
            await _dbService._filmCollection.DeleteManyAsync(f => true);
        }

        public async Task DeleteFilmAsync(string filmCode)
        {
            await _dbService._filmCollection.DeleteOneAsync(f => f.FilmCode == filmCode);
        }

        public async Task<List<Film>> GetFilmsAsync()
        {
            var films = await _dbService._filmCollection.Find(f => true).ToListAsync();
            return films;
        }

        public async Task<Film> GetSpecificFilm(string filmCode)
        {
            var film = await _dbService._filmCollection.Find(f => f.FilmCode == filmCode).FirstOrDefaultAsync();
            return film;
        }
    }
}
