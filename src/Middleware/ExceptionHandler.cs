using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;

namespace Middleware
{
    public static class ExceptionHandler
    {
        public static void Handle(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                if (context != null)
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;

                        // https://tools.ietf.org/html/rfc7807#section-3.1
                        var problemDetails = new ProblemDetails
                        {
                            Type = $"https://example.com/problem-types/{exception.GetType().Name}",
                            Title = "An unexpected error occurred!",
                            Detail = "Something went wrong",
                            Instance = errorFeature switch
                            {
                                ExceptionHandlerFeature e => e.Path,
                                _ => "unknown"
                            },
                            Status = StatusCodes.Status400BadRequest,
                            Extensions = { ["trace"] = Activity.Current?.Id ?? context.TraceIdentifier }
                        };

                        switch (exception)
                        {
                            case ValidationException validationException:
                                problemDetails.Status = StatusCodes.Status400BadRequest;
                                problemDetails.Title = "One or more validation errors occurred";
                                problemDetails.Detail = "The request contains invalid parameters. More information can be found in the errors.";
                                problemDetails.Extensions["errors"] = validationException.Errors;
                                break;
                        }

                        context.Response.ContentType = "application/problem+json";
                        context.Response.StatusCode = problemDetails.Status.Value;
                        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                        {
                            NoCache = true,
                        };

                        await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
                    }
                }
            });
        }
    }
}
