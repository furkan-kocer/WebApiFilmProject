using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace FilmProject.Services.Extensions
{
    public static class GlobalExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature is not null)
                    {
                        Log.Logger.Error("Something went wrong: {contextFeature.Error}",contextFeature.Error);

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            statusCode = context.Response.StatusCode,
                            message = "Internal Server Error"
                        }.ToString()
                        );
                    }
                });
            });
        }
    }
}
