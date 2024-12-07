using Identity.Domain.JWTDto;
using Identity.Domain.Modal;
using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        public IActionResult GenerateToken([FromBody]TokenGenerationRequest request,IOptions<JWTSettings> config)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tokenResponse = _identityService.TokenGenerate(request, config);
            return Ok(tokenResponse);
        }
    }
}
