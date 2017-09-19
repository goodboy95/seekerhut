using Microsoft.AspNetCore.Builder;

namespace Middleware
{
    public static class ErrorPageExtentions
    {
        public static IApplicationBuilder UseErrorPage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorPageMiddleware>();
        }
    }
}