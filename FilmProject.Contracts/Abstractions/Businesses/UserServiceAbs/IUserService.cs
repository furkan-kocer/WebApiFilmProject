using FilmProject.Contracts.DataTransferObjects.User;
namespace FilmProject.Contracts.Abstractions.Businesses.UserServiceAbs
{
    public interface IUserService
    {
        Task<GenericResponseBase<string>> RegisterUser(UserRegisterRequest registerRequest);
        Task<GenericResponseBase<UserLoginResponse>> Login(UserLoginRequest registerRequest);
    }
}
