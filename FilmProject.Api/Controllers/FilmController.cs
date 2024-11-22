using FilmProject.Api.ApiReturnControls;
using FilmProject.DataAccess.DataTransferObjects.Film;
using FilmProject.Services.Businesses.FilmService;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FilmProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;
        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilms()
        {
            Log.Logger.ForContext("EventId", 1001).Verbose("Get method called with the name of {methodName}", nameof(GetAllFilms));
            var films = await _filmService.GetAllFilmsAsync();
            if (films.result == false)
            {
                return BadRequest(films);
            }
            return Ok(films);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilm(FilmRequest filmRequest)
        {
            var response = await _filmService.CreateFilm(filmRequest);
            return Created("", response);
        }

        [HttpGet("{filmCode}")]
        public async Task<IActionResult> GetFilmByCode([FromRoute] string filmCode)
        {
            if (string.IsNullOrEmpty(filmCode))
            {
                var problemDetails = CustomProblemDetailsFactory.CreateProblemDetails(
                httpContext: HttpContext,
                type: "Bad Request",
                title: "Null or Empty.",
                detail: "FilmCode cannot be null or empty.",
                statusCode: StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
            var film = await _filmService.GetSpecificFilmAsync(filmCode);
            if (film.data == null)
            {
                return NotFound(film);
            }
            return Ok(film);
        }
        [HttpPut("{filmCode}")]
        public async Task<IActionResult> UpdateFilmRecord([FromBody] FilmRequest filmRequest, [FromRoute] string filmCode)
        {
            if (string.IsNullOrEmpty(filmCode))
            {
                var problemDetails = CustomProblemDetailsFactory.CreateProblemDetails(
                httpContext: HttpContext,
                type: "Bad Request",
                title: "Null or Empty.",
                detail: "FilmCode cannot be null or empty.",
                statusCode: StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
            var filmAvaliable = await _filmService.GetSpecificFilmAsync(filmCode);
            if (filmAvaliable.data == null)
            {
                return NotFound(filmAvaliable);
            }
            var response = await _filmService.UpdateFilmByCode(filmRequest, filmCode);
            if (response.result == false)   
            {
                return BadRequest(response);
            }
            return NoContent();
        }
        [HttpDelete("{filmCode}")]
        public async Task<IActionResult> DeleteFilmByCode([FromRoute] string filmCode)
        {
            if (string.IsNullOrEmpty(filmCode))
            {
                var problemDetails = CustomProblemDetailsFactory.CreateProblemDetails(
                httpContext: HttpContext,
                type: "Bad Request",
                title: "Null or Empty.",
                detail: "FilmCode cannot be null or empty.",
                statusCode: StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
            var filmAvaliable = await _filmService.GetSpecificFilmAsync(filmCode);
            if (filmAvaliable.data == null)
            {
                return NotFound(filmAvaliable);
            }
            await _filmService.DeleteSpecificFilmAsync(filmCode);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllFilms()
        {
            await _filmService.DeleteAllFilms();
            return NoContent();
        }
    }
}
