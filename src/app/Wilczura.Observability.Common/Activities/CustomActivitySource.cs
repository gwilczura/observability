using System.Diagnostics;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Activities;

public static class CustomActivitySource
{
    public static readonly Lazy<ActivitySource> Source = new Lazy<ActivitySource>(() =>
                new ActivitySource(ObservabilityConsts.DefaultListenerName));
}
