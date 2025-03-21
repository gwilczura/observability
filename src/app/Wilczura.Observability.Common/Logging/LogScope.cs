using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Wilczura.Observability.Common.Activities;

namespace Wilczura.Observability.Common.Logging;


// TODO: SHOW P2.1 - LogScope
public class LogScope : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogInfo _logInfo;
    private readonly Stopwatch _stopwatch = new();
    private readonly LogLevel _level;
    private readonly EventId? _eventId;
    private readonly Activity? _activity;

    public LogInfo LogInfo { get {  return _logInfo; } }

    public LogScope(
        ILogger logger, 
        LogInfo logInfo,
        LogLevel? logLevel = null, 
        EventId? eventId = null,
        string? activityName = null)
    {
        _stopwatch.Start();
        if(activityName != null)
        {
            // TODO: SHOW P2.2 - LogScope and CustomActivitySource
            _activity = CustomActivitySource.Source.Value.StartActivity(activityName, ActivityKind.Internal);
        }
        _logger = logger;
        _logInfo = logInfo;
        _level = (logLevel ?? LogLevel.Information);
        _eventId = eventId;
    }


    public void Dispose()
    {
        _stopwatch.Stop();
        _logInfo.EventDuration = _stopwatch.ElapsedMilliseconds;
        if (_activity != null)
        {
            _activity.SetEndTime(DateTime.UtcNow);
        }
        // TODO: SHOW P2.3 - how are logs handled
        _logger.Log(_level, _logInfo, _eventId);
        if(_activity != null)
        {
            _activity.Dispose();
        }
    }
}
