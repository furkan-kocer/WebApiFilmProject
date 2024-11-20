using Microsoft.AspNetCore.Mvc;

namespace FilmProject.Api.ApiReturnControls
{
    public class CustomProblemDetailsFactory : ProblemDetails
    {
        public static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string title = null,
        string type = null,
        string detail = null,
        string instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode ?? 500,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };
            return problemDetails;
        }
    }
}
