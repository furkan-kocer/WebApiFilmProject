using FilmProject.Contracts;
using FilmProject.Contracts.Abstractions.Businesses.UserServiceAbs;
using FilmProject.Contracts.DataTransferObjects.User;
using FilmProject.DataAccess.CollectionRepositories.UserCollection;
using FilmProject.DataAccess.Entities;
using FilmProject.Services.Businesses.ExternalApi;
using FilmProject.Services.Helpers;
using Mapster;
using Microsoft.Extensions.Options;

namespace FilmProject.Services.Businesses.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserCollectionRepository _userRepository;
        private readonly IExternalApiService _externalApiService;
        private readonly string _exampleApiEndpoint;
        public UserService(IUserCollectionRepository userRepository, IExternalApiService externalApiService, IOptions<ExternalApiSettings> externalApiSettings)
        {
            _userRepository = userRepository;
            _externalApiService = externalApiService;
            _exampleApiEndpoint = externalApiSettings.Value.Kernel.URLHttps;
        }
        //TODO
        public async Task<GenericResponseBase<string>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var user = registerRequest.Adapt<User>();
            var checkUserAvailable = await _userRepository.CheckUserExist(user);
            if (!checkUserAvailable.Any())
            {
                user.updatedDate = DateTime.UtcNow;
                user.createdDate = DateTime.UtcNow;
                user.status = true;
                await _userRepository.RegisterUserAsync(user);
                return GenericResponseBase<string>.Success("Successfully registered.");
            }
            if (checkUserAvailable.Count == 1)
            {
                return GenericResponseBase<string>.Error(checkUserAvailable[0]);
            }
            return GenericResponseBase<string>.ErrorList(checkUserAvailable);
        }
        public async Task<GenericResponseBase<UserLoginResponse>> Login(UserLoginRequest userLoginRequest)
        {
            var isEmail = UserHelper.CheckEmailorPhoneNum(userLoginRequest.Field);
            var user = userLoginRequest.Adapt<User>();
            var checkUserResponse = await _userRepository.IsLoginInputMatch(isEmail, user);
            if (!checkUserResponse)
                return GenericResponseBase<UserLoginResponse>.Error($"Your {isEmail} or password incorrect.");
            var getUserResponse = await _userRepository.GetMatchedUserAsync(isEmail, user);
            var userTokenResponse = getUserResponse.Adapt<UserLoginTokenResponse>();
            var endPoint = _exampleApiEndpoint + "/api/Identity/token";
            var getTokenResponse = await _externalApiService.PostToExternalApiAsync<UserLoginResponse>(endPoint, userTokenResponse);
            return GenericResponseBase<UserLoginResponse>.Success(getTokenResponse);

        }
    }
}
