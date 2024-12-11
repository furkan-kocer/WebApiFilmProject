using Identity.Domain.JWTDto;
namespace Identity.Services
{
    public interface _IdentityService
    {
        TokenGenerationResponse CreateToken(TokenGenerationRequest request);
        GenericResponseBase<NewTokenDto> RefreshTokens(RefreshTokenGenerationRequest request);
    }
}
