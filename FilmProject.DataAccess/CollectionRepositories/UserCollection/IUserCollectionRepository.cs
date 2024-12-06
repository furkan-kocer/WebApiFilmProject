using FilmProject.DataAccess.Entities;

namespace FilmProject.DataAccess.CollectionRepositories.UserCollection
{
    public interface IUserCollectionRepository
    {
        Task<List<string>> CheckUserExist(User user);
        Task RegisterUserAsync(User user);
        Task<bool> IsLoginInputMatch(string field, User user);
        Task<User> GetMatchedUserAsync(string field, User user);
    }
}
