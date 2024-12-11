using FilmProject.Contracts;
using FilmProject.Contracts.Abstractions.Businesses.UserServiceAbs;
using FilmProject.Contracts.DataTransferObjects.Token;
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
        private readonly string _identityApiHttps;
        private readonly string _identityApiTokenEndpoint;
        private readonly string _identityApiCreateTokensEndpoint;
        public UserService(IUserCollectionRepository userRepository, IExternalApiService externalApiService, IOptions<ExternalApiSettings> externalApiSettings)
        {
            _userRepository = userRepository;
            _externalApiService = externalApiService;
            _identityApiHttps = externalApiSettings.Value.GetApiRequestByName("IdentityApiRequest").Kernel.URLHttps;
            _identityApiTokenEndpoint = externalApiSettings.Value.GetApiRequestByName("IdentityApiRequest").GetFullEndpointUrl("token");
            _identityApiCreateTokensEndpoint = externalApiSettings.Value.GetApiRequestByName("IdentityApiRequest").GetFullEndpointUrl("refreshtoken");
        }
        //TODO
        public async Task<GenericResponseBase<string>> RegisterUser(UserRegisterRequest registerRequest)
        {
            var user = registerRequest.Adapt<User>();
            var getMatchedUsers = await _userRepository.CheckUserExist(user);
            var userHasConflicts = _userRepository.CheckHasConflicts(getMatchedUsers, user);
            if (!userHasConflicts.Any())
            {
                user.updatedDate = DateTime.UtcNow;
                user.createdDate = DateTime.UtcNow;
                user.status = true;
                await _userRepository.RegisterUserAsync(user);
                return GenericResponseBase<string>.Success("Successfully registered.");
            }
            if (userHasConflicts.Count == 1)
            {
                return GenericResponseBase<string>.Error(userHasConflicts[0]);
            }
            return GenericResponseBase<string>.ErrorList(userHasConflicts);
        }
        public async Task<GenericResponseBase<UserLoginResponse>> Login(UserLoginRequest userLoginRequest)
        {
            var isEmail = UserHelper.CheckEmailorPhoneNum(userLoginRequest.Field);
            var user = userLoginRequest.Adapt<User>();
            var checkUserResponse = await _userRepository.IsLoginInputMatch(isEmail, user);
            if (!checkUserResponse)
                return GenericResponseBase<UserLoginResponse>.Error($"Your {isEmail} or password is incorrect.");
            var getUserResponse = await _userRepository.GetMatchedUserAsync(isEmail, user);
            var userTokenResponse = getUserResponse.Adapt<UserLoginTokenResponse>();
            var endPoint = _identityApiHttps + _identityApiTokenEndpoint;
            var getTokenResponse = await _externalApiService.PostToExternalApiAsync<LoginTokenResponse>(endPoint, userTokenResponse);
            if (getTokenResponse == null)
            {
                throw new ArgumentNullException(nameof(getTokenResponse) + " returned null from identity request.");
            }
            var isRefreshTokenAdded = await _userRepository.UpdateRefreshTokenAsync(getTokenResponse.RefreshToken, getTokenResponse.RefreshTokenExpiryTime, getUserResponse);
            if (!isRefreshTokenAdded)
            {
                throw new Exception($"Something went wrong at {nameof(isRefreshTokenAdded)} method.");
            }
            var userLoginResponse = getTokenResponse.Adapt<UserLoginResponse>();
            return GenericResponseBase<UserLoginResponse>.Success(userLoginResponse);
        }

        public async Task<GenericResponseBase<NewTokensResponse>> GenerateNewTokens(TokenRequest tokenRequest)
        {
            var isRefreshTokenValid = await _userRepository.CheckRefreshTokenValid(tokenRequest.RefreshToken);
            if (!isRefreshTokenValid)
                return GenericResponseBase<NewTokensResponse>.Error($"Refresh token is invalid.");
            var user = await _userRepository.GetUserByRefreshToken(tokenRequest.RefreshToken);
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return GenericResponseBase<NewTokensResponse>.Error("Refresh token expired.");
            var userTokenResponse = user.Adapt<NewTokensRequest>();
            userTokenResponse.Token = tokenRequest.Token;
            var endpoint = _identityApiHttps + _identityApiCreateTokensEndpoint;
            var getTokenResponse = await _externalApiService.PostToExternalApiAsync<NewTokensResponse>(endpoint, userTokenResponse);
            await _userRepository.UpdateRefreshTokenValueAsync(getTokenResponse.RefreshToken, user);
            return GenericResponseBase<NewTokensResponse>.Success(getTokenResponse); 
        }
    }
}
