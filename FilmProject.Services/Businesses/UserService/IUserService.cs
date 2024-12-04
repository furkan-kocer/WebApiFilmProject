using FilmProject.DataAccess.DataTransferObjects.User;

namespace FilmProject.Services.Businesses.UserService
{
    public interface IUserService
    {
        Task<GenericResponseBase<string>> RegisterUser(UserRegisterRequest registerRequest);
        Task<GenericResponseBase<UserLoginResponse>> Login(UserLoginRequest registerRequest);
    }
}
