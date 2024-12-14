using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Logging;

namespace Wilczura.Observability.Common.ServiceBus;

public class CustomLoggerSpecification<T> : IPipeSpecification<T> where T : class, PipeContext
{
    private readonly ILogger<CustomLoggerFilter<T>> _logger;

    public CustomLoggerSpecification(ILogger<CustomLoggerFilter<T>> logger)
    {
        _logger = logger;
    }

    public void Apply(IPipeBuilder<T> builder)
    {
        builder.AddFilter(new CustomLoggerFilter<T>(_logger));

    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}