using FilmProject.DataAccess.DataTransferObjects.User;
using FilmProject.Services.Businesses.UserService;
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
    }
}
