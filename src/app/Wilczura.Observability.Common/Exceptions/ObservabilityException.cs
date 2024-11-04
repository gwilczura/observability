namespace Wilczura.Observability.Common.Exceptions;

public class ObservabilityException(string? message = null, Exception? innerException = null) : Exception(message, innerException)
{
}
