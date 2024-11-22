using FilmProject.Api.FilterError;
using FilmProject.DataAccess.FilterErrors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace FilmProject.Services.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {

                var errors = context.ModelState
                    .Where(er => er.Value.Errors.Any())
                    .Select(ms => new FilterErrorModel
                    {
                        fieldName = ms.Key,
                        messages = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                var filterErrorDetails = new FilterErrorDetails
                {
                    type = "BadRequest",
                    title = "Validation Failed",
                    detail = "One or more validation errors occured.",
                    statusCode = StatusCodes.Status400BadRequest,
                    errorModelResponse = new FilterErrorModelResponse
                    {
                        Errors = errors
                    }
                };
                #region with ProblemDetails Class Return
                //var problemDetails = new ProblemDetails
                //{
                //    Title = "Validation Failed",
                //    Detail = "One or more validation errors occurred.",
                //    Status = StatusCodes.Status400BadRequest,
                //    Extensions = { ["errors"] = errors }
                //};
                #endregion
                Log.Logger.Error("Validation failed: {@Errors}", filterErrorDetails);
                context.Result = new BadRequestObjectResult(filterErrorDetails);
                return;
            }
            await next();
        }
    }
}
