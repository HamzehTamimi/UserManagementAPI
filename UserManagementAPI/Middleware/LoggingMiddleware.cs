using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            Console.WriteLine($"[Request] {request.Method} {request.Path}");

            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            Console.WriteLine($"[Response] {response.StatusCode} ({stopwatch.ElapsedMilliseconds} ms)");
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}