using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handling {RequestName}. Request: {@Request}", typeof(TRequest).Name, request);

            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            if (timer.Elapsed > TimeSpan.FromSeconds(3))
            {
                logger.LogWarning(
                    "[PERFORMANCE] {RequestName} took {ElapsedMilliseconds} ms",
                    typeof(TRequest).Name,
                    timer.ElapsedMilliseconds);
            }

            logger.LogInformation("[END] Handled {RequestName} in {ElapsedMilliseconds} ms", typeof(TRequest).Name, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
