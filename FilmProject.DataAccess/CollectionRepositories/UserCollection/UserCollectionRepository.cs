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

        public async Task<List<string>> CheckUserExist(User user)
        {
            var conflictingUsers = await _dbService._userCollection.Find(u => u.Email == user.Email || user.PhoneNumber != null ?
                                                            u.PhoneNumber == user.PhoneNumber : false).ToListAsync();
            // Analyze conflicts and collect conflict types
            var conflicts = new List<string>();
            if (conflictingUsers.Any())
            {
                foreach (var users in conflictingUsers)
                {
                    if (user.Email == user.Email)
                        conflicts.Add("Email already exist.");
                    if (user.PhoneNumber == user.PhoneNumber && user.PhoneNumber != null)
                        conflicts.Add("PhoneNumber already exist.");
                }
            }
            return conflicts;
        }

        public async Task<bool> IsLoginInputMatch(string field,User user)
        {
            var fieldCondition = field == "Email" ? user.Email : user.PhoneNumber;
            var isMatching = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == fieldCondition
                && u.Password == user.Password).AnyAsync();
            return isMatching;
        }
        public async Task<User> GetMatchedUserAsync(string field, User user)
        {
            var fieldCondition = field == "Email" ? user.Email : user.PhoneNumber;
            var projection = Builders<User>.Projection.Expression(user => new User
            {
              Id = user.Id,
              Email = user.Email,
              PhoneNumber = user.PhoneNumber,
              Role = user.Role
            });
            var getMatchedUser = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == fieldCondition
                && u.Password == user.Password).Project(projection).FirstOrDefaultAsync();
            return user;
        }
        public async Task RegisterUserAsync(User user)
        {
            await _dbService._userCollection.InsertOneAsync(user);
        }
    }
}
