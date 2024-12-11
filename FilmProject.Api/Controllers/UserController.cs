using FilmProject.Contracts.Abstractions.Businesses.UserServiceAbs;
using FilmProject.Contracts.DataTransferObjects.Token;
using FilmProject.Contracts.DataTransferObjects.User;
using Microsoft.AspNetCore.Mvc;

namespace FilmProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterRequest registerRequest)
        {
            var registerResponse = await _userService.RegisterUser(registerRequest);

            if (!registerResponse.result)
            {
                return BadRequest(registerResponse);
            }
            return Created("", registerResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest userLoginRequest)
        {
            var loginResponse = await _userService.Login(userLoginRequest);

            if (!loginResponse.result)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }
        [HttpPost]  
        public async Task<IActionResult> RefreshToken(TokenRequest tokenRequest)
        {
            var refresTokenResponse = await _userService.GenerateNewTokens(tokenRequest);
            if (!refresTokenResponse.result)
            {
                return BadRequest(refresTokenResponse);
            }
            return Ok(refresTokenResponse);
        }
    }
}
