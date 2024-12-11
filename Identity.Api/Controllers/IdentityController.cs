using Identity.Domain.JWTDto;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;
namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly _IdentityService _identityService;
        public IdentityController(_IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tokenResponse = _identityService.CreateToken(request);
            return Ok(tokenResponse);
        }
        [HttpPost("refreshtoken")]
        public IActionResult RefreshTokens([FromBody] RefreshTokenGenerationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tokenDtoResponse = _identityService.RefreshTokens(request);
            if (tokenDtoResponse.result == false)
            {
                return BadRequest(tokenDtoResponse.errorDetail);
            }
            return Ok(tokenDtoResponse.data);
        }
    }
}
