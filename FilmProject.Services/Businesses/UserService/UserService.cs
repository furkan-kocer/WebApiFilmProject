using FilmProject.DataAccess.CollectionRepositories.UserCollection;
using FilmProject.DataAccess.DataTransferObjects.User;
using FilmProject.DataAccess.Entities;
using Mapster;
namespace FilmProject.Services.Businesses.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserCollectionRepository _userRepository;
        public UserService(IUserCollectionRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GenericResponseBase<string>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var checkUserAvailable = await _userRepository.CheckUserExist(registerRequest);
            if (!checkUserAvailable.Any())
            {
                var user = registerRequest.Adapt<User>();
                user.updatedDate = DateTime.UtcNow;
                user.createdDate = DateTime.UtcNow;
                user.status = true;
                await _userRepository.RegisterUserAsync(user);
                return GenericResponseBase<string>.Success("Successfully registered.");
            }
            return GenericResponseBase<string>.ErrorList(checkUserAvailable);
        }
    }
}
