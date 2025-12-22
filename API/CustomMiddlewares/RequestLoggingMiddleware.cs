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
            var startTime = DateTime.UtcNow;

            await _next(context);

            var duration = DateTime.UtcNow - startTime;
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation(
                "HTTP {Method} {Path} {QueryString} responded {StatusCode} in {Duration} ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                statusCode,
                duration.TotalMilliseconds);
        }
    }
}
