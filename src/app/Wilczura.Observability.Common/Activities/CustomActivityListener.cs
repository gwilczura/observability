using System.Diagnostics;

namespace Wilczura.Observability.Common.Activities;

public class CustomActivityListener : IDisposable
{
    private readonly string _activityName;
    private readonly ActivityListener _listener;

    // TODO: SHOW P1 - ActivityListener - required by ActivitySource.Create()
    // trivia - new Activity() works fine without it
    public CustomActivityListener(string activitySourceName)
    {
        _activityName = activitySourceName;
        _listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == _activityName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStarted = OnActivityStarted,
            ActivityStopped = OnActivityStopped
        };
        ActivitySource.AddActivityListener(_listener);

    }

    private void OnActivityStarted(Activity activity)
    {
        // ignore
    }

    private void OnActivityStopped(Activity activity)
    {
        // ignore
    }

    public void Dispose()
    {
        _listener.Dispose();
    }
}
