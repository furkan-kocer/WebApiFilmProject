using FilmProject.DataAccess.Entities;

namespace FilmProject.DataAccess.CollectionRepositories.UserCollection
{
    public interface IUserCollectionRepository
    {
        Task<List<User>> CheckUserExist(User user);
        Task RegisterUserAsync(User user);
        Task<bool> IsLoginInputMatch(string field, User user);
        Task<User> GetMatchedUserAsync(string field, User user);
        List<string> CheckHasConflicts(List<User> conflictingUsers, User user);
        Task<bool> UpdateRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, User user);
        Task<bool> CheckRefreshTokenValid(string refreshToken);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task UpdateRefreshTokenValueAsync(string refreshToken, User user);
    }
}
