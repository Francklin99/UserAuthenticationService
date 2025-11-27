using System.Diagnostics;

namespace API.Middlewares
{
    public class RequestTimeLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeLoggingMiddleware> _logger;

        public RequestTimeLoggingMiddleware(RequestDelegate next, ILogger<RequestTimeLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var method = context.Request.Method;
            var path = context.Request.Path;
            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            // ---- Timestamp ----
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"[{DateTime.Now:HH:mm:ss} INF] HTTP ");
            Console.ResetColor();

            // ---- HTTP Method (Yellow) ----
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{method} ");
            Console.ResetColor();

            // ---- Path (Cyan) ----
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{path} ");
            Console.ResetColor();

            Console.Write("responded ");

            // ---- Status Code ----
            if (statusCode >= 200 && statusCode < 300)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (statusCode >= 400 && statusCode < 500)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (statusCode >= 500)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"{statusCode} ");
            Console.ResetColor();

            // ---- Duration ----

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"in {elapsedMilliseconds:F4} ms");
            Console.ResetColor();
        }
    }
}
