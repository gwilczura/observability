using Microsoft.Extensions.Logging;

namespace Wilczura.Observability.Common.Logging;

public static class LogEvents
{
    public static readonly EventId Custom = new(1, "Custom");
    public static readonly EventId WebRequest = new(2, "WebRequest");
    public static readonly EventId ConsumeMessage = new(3, "ConsumeMessage");
    public static readonly EventId SendMessage = new(4, "SendMessage");
    public static readonly EventId PublishMessage = new(5, "PublishMessage");
}
