using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Host.MassTransit;

namespace Wilczura.Observability.Common.Host.Extensions;

public static class MassTransitExtensions
{
    public static void UseCustomConsumeLogger<T>(this IPipeConfigurator<T> configurator, IBusRegistrationContext context)
        where T : class, PipeContext
    {
        configurator.AddPipeSpecification(new CustomLoggerSpecification<T>(context.GetRequiredService<ILogger<CustomLoggerFilter<T>>>()));
    }
}
