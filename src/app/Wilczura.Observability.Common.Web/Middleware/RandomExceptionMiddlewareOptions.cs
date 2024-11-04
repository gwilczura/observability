namespace Wilczura.Observability.Common.Web.Middleware;

public class RandomExceptionMiddlewareOptions
{
    public const string ConfigurationKey = "RandomExceptionMiddleware";

    public double? Rate { get; set; } = 0;
}
