using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string TOKEN = "mysecrettoken123";

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var token) || token != $"Bearer {TOKEN}")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
                return;
            }

            await _next(context);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}