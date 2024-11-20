using FilmProject.DataAccess.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace FilmProject.DataAccess
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Film> _filmCollection;
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.connectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.databaseName);
            _filmCollection = database.GetCollection<Film>(mongoDBSettings.Value.collections.filmCollection);
        }
    }
}
