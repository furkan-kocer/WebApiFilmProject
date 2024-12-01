using FilmProject.DataAccess.DataTransferObjects.User;
using FilmProject.DataAccess.Entities;
using MongoDB.Driver;
namespace FilmProject.DataAccess.CollectionRepositories.UserCollection
{
    public class UserCollectionRepository : IUserCollectionRepository
    {
        private readonly MongoDBService _dbService;
        public UserCollectionRepository(MongoDBService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<string>> CheckUserExist(UserRegisterRequest userRequest)
        {
            var conflictingUsers = await _dbService._userCollection.Find(u => u.Email == userRequest.Email ||
                                                            u.PhoneNumber == userRequest.PhoneNumber).ToListAsync();
            // Analyze conflicts and collect conflict types
            var conflicts = new List<string>();
            if (conflictingUsers.Any())
            {
                foreach (var user in conflictingUsers)
                {
                    if (user.Email == userRequest.Email)
                        conflicts.Add("Email already exist.");
                    if (user.PhoneNumber == userRequest.PhoneNumber && user.PhoneNumber != null)
                        conflicts.Add("PhoneNumber already exist.");
                }
            }
            return conflicts;
        }

        public async Task RegisterUserAsync(User user)
        {
            await _dbService._userCollection.InsertOneAsync(user);
        }
    }
}
