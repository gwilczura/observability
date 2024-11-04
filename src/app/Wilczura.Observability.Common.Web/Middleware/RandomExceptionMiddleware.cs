using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Wilczura.Observability.Common.Exceptions;

namespace Wilczura.Observability.Common.Web.Middleware;

public class RandomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Random _random;

    // TODO: SHOW P4 - random exception middleware
    public RandomExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _random = new Random();
    }

    public async Task InvokeAsync(HttpContext context, IOptionsSnapshot<RandomExceptionMiddlewareOptions> optionsSnapshot)
    {
        string[] ignorePaths = [string.Empty, "/"];
        if(!ignorePaths.Contains(context.Request.Path.Value))
        {
            // options snapshot generates new value with each request - allows change without service restart
            var rate = double.Min(1, optionsSnapshot?.Value?.Rate ?? 0);
            var max = 1000;
            if (rate > 0)
            {
                var randomNumber = _random.Next(max);
                if (randomNumber <= (max * rate))
                {
                    throw new ObservabilityException("RandomException");
                }
            }
        }

        await _next(context);
    }
}
