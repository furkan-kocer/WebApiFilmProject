using Identity.Domain.JWTDto;
using Identity.Domain.Modal;
using Microsoft.Extensions.Options;

namespace Identity.Services
{
    public interface _IdentityService
    {
        TokenGenerationResponse TokenGenerate(TokenGenerationRequest request);
    }
}
