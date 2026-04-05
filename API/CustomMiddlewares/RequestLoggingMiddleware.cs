using System.Diagnostics;

namespace API.CustomMiddlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await _next(context);

            stopWatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} {QueryString} responded {StatusCode} in {Duration} ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Response.StatusCode,
                stopWatch.ElapsedMilliseconds);
        }
    }
}
