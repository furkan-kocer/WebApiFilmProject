using FilmProject.DataAccess.DataTransferObjects.User;
using FilmProject.DataAccess.Entities;
using FilmProject.DataAccess.Helpers;
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
            var conflictingUsers = await _dbService._userCollection.Find(u => u.Email == userRequest.Email || userRequest.PhoneNumber != null ?
                                                            u.PhoneNumber == userRequest.PhoneNumber : false).ToListAsync();
            // Analyze conflicts and collect conflict types
            var conflicts = new List<string>();
            if (conflictingUsers.Any())
            {
                foreach (var user in conflictingUsers)
                {
                    if (user.Email == userRequest.Email)
                        conflicts.Add("Email already exist.");
                    if (user.PhoneNumber == userRequest.PhoneNumber && userRequest.PhoneNumber != null)
                        conflicts.Add("PhoneNumber already exist.");
                }
            }
            return conflicts;
        }

        public async Task<bool> IsLoginInputMatch(string field,UserLoginRequest userLoginRequest)
        {
            var isMatching = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == userLoginRequest.Field
                && u.Password == userLoginRequest.Password).AnyAsync();
            return isMatching;
        }
        public async Task<UserLoginTokenResponse> GetMatchedUserAsync(string field, UserLoginRequest userLoginRequest)
        {
            var projection = Builders<User>.Projection.Expression(user => new UserLoginTokenResponse
            {
              userID = user.Id,
              email = user.Email,
              phoneNumber = user.PhoneNumber,
              Role = user.Role
            });
            var user = await _dbService._userCollection.Find(u => (field == "Email" ? u.Email : u.PhoneNumber) == userLoginRequest.Field
                && u.Password == userLoginRequest.Password).Project(projection).FirstOrDefaultAsync();
            return user;
        }
        public async Task RegisterUserAsync(User user)
        {
            await _dbService._userCollection.InsertOneAsync(user);
        }
    }
}
