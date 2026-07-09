using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace Shared.ExceptionHandlers
{
    public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment)
    : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            ProblemDetails problem;

            switch (exception)
            {
                case ValidationException validationException:
                    {
                        var errors = validationException.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray());

                        problem = new ValidationProblemDetails(errors)
                        {
                            Title = "One or more validation errors occurred.",
                            Status = StatusCodes.Status400BadRequest
                        };

                        break;
                    }

                case NotFoundException:
                    {
                        problem = new ProblemDetails
                        {
                            Title = "Resource not found.",
                            Status = StatusCodes.Status404NotFound
                        };

                        break;
                    }

                case DomainException domainException:
                    {
                        problem = new ProblemDetails
                        {
                            Title = domainException.Message,
                            Detail = exception.Message,
                            Status = StatusCodes.Status422UnprocessableEntity
                        };

                        break;
                    }

                default:
                    {
                        problem = new ProblemDetails
                        {
                            Title = "An unexpected error occurred.",
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = environment.IsDevelopment()
                                ? exception.ToString()
                                : null
                        };

                        break;
                    }
            }
            httpContext.Response.StatusCode = problem.Status!.Value;

            var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem,
                Exception = exception
            });

            return true;
        }
    }
}
