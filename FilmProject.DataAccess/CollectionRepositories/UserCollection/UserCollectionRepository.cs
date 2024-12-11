using FilmProject.DataAccess.Entities;
using FilmProject.DataAccess.Helpers;
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

        public async Task<List<User>> CheckUserExist(User user)
        {
            var getMatchedUsers = await _dbService._userCollection.Find
                (u => u.Email == user.Email ||
                (string.IsNullOrEmpty(user.PhoneNumber) ? false : u.PhoneNumber == user.PhoneNumber))
                .ToListAsync();
            return getMatchedUsers;
        }
        public List<string> CheckHasConflicts(List<User> conflictingUsers, User user)
        {
            return UserRepositoryHelper.AnalyzeConflicts(conflictingUsers, user);
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

        public async Task<bool> UpdateRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update.Set(u => u.RefreshToken, refreshToken)
                                              .Set(u => u.RefreshTokenExpiryTime, refreshTokenExpiryTime);
            var result = await _dbService._userCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        public async Task UpdateRefreshTokenValueAsync(string refreshToken, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update.Set(u => u.RefreshToken, refreshToken);
            await _dbService._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> CheckRefreshTokenValid(string refreshToken)
        {
            var isTokenAvaliable = await _dbService._userCollection.Find(u => u.RefreshToken == refreshToken).AnyAsync();
            return isTokenAvaliable;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var user = await _dbService._userCollection.Find(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
            return user;
        }
    }
}
