using Microsoft.AspNetCore.Builder;

namespace SimpleWebApi.Infrastructure.Middleware
{
    public static class Extensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }
    }
}