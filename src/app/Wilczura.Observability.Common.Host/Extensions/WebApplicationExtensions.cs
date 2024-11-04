using Microsoft.AspNetCore.Builder;
using Wilczura.Observability.Common.Web.Middleware;

namespace Wilczura.Observability.Common.Host.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseObservabilityDefaults(this WebApplication app)
    {
        // TODO: SHOW P9 - use middleware
        app.UseRequestLogging();
        app.UseHttpsRedirection();
        app.UseRandomException();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        return app;
    }

    // TODO: SHOW P5 - Use Random Exception
    public static IApplicationBuilder UseRandomException(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RandomExceptionMiddleware>();
    }

    // TODO: SHOW P3 - Use Request Logging (ASP.NET)
    public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
