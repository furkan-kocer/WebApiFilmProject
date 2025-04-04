﻿using FilmProject.DataAccess.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FilmProject.DataAccess
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Film> _filmCollection;
        public readonly IMongoCollection<User> _userCollection;
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.connectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.databaseName);
            _filmCollection = database.GetCollection<Film>(mongoDBSettings.Value.collections.filmCollection);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.collections.userCollection);
        }
    }
}
