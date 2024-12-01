using FilmProject.DataAccess.DataTransferObjects.User;
using FilmProject.DataAccess.Entities;

namespace FilmProject.DataAccess.CollectionRepositories.UserCollection
{
    public interface IUserCollectionRepository
    {
        Task<List<string>> CheckUserExist(UserRegisterRequest userRequest);
        Task RegisterUserAsync(User user);
    }
}
