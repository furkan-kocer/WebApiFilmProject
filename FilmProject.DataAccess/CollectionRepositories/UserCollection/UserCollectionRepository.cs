using FilmProject.DataAccess.Entities;
using Mapster;
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
            var conflictingUsers = await _dbService._userCollection.Find
                (u => u.Email == user.Email ||
                (string.IsNullOrEmpty(user.PhoneNumber) ? false : u.PhoneNumber == user.PhoneNumber))
                .ToListAsync();
            // Analyze conflicts and collect conflict types
            var conflicts = new List<string>();
            if (conflictingUsers.Any())
            {
                string emailExist = "Email already exist.";
                string phoneExist = "PhoneNumber already exist.";
                foreach (var users in conflictingUsers)
                {
                    if (users.Email == user.Email && !(conflicts.Contains(emailExist)))
                        conflicts.Add(emailExist);
                    if (users.PhoneNumber == user.PhoneNumber && !(string.IsNullOrEmpty(user.PhoneNumber)) && !(conflicts.Contains(phoneExist)))
                        conflicts.Add(phoneExist);
                    if (conflicts.Contains(emailExist) && conflicts.Contains(phoneExist))
                        break;
                }
            }
            return conflicts;
        }

        public async Task<bool> IsLoginInputMatch(string field, User user)
        {
            var fieldCondition = field == "Email" ? user.Email : user.PhoneNumber;
            var isMatching = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == fieldCondition
                && u.Password == user.Password).AnyAsync();
            return isMatching;
        }
        public async Task<User> GetMatchedUserAsync(string field, User user)
        {
            var fieldCondition = field == "Email" ? user.Email : user.PhoneNumber;
            var projection = Builders<User>.Projection.Expression(user => new
            {
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.Role
            });
            var getMatchedUser = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == fieldCondition
                && u.Password == user.Password).Project(projection).FirstOrDefaultAsync();
            var matchedUser = getMatchedUser.Adapt<User>();
            return matchedUser;
        }
        public async Task RegisterUserAsync(User user)
        {
            await _dbService._userCollection.InsertOneAsync(user);
        }
    }
}
