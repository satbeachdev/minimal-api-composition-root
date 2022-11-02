using Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Middleware
{
    public class ApiPerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggingService _logger;

        /// <summary>
        /// Logs the time spent in the API handler
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <remarks>This MUST be the last middleware in the chain!</remarks>
        public ApiPerformanceMiddleware(RequestDelegate next, ILoggingService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            // Invoke the next middleware in the pipeline
            await _next(context);

            // Log the time spent in the API handler
            _logger.Log($"API {context.Request.Path} took {sw.Elapsed} to execute.");
        }
    }
}