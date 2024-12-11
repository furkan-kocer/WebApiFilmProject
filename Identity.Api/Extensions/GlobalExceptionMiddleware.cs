using Identity.Domain.Modal;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

namespace Identity.Api.Extensions
{
    public static class GlobalExceptionMiddleware
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
                    if (contextFeature is not null)
                    {
                        var contextError = contextFeature.Error;
                        Log.Logger.Error("Something went wrong: {contextError}", contextError);

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
