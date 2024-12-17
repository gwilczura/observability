using System.Reflection;

namespace Wilczura.Observability.Common
{

    public static class SystemInfo
    {
        private static string _info = string.Empty;
        private static readonly object _syncLocker = new();
        public static string GetInfo()
        {
            lock (_syncLocker)
            {
                if (string.IsNullOrEmpty(_info))
                {

                    var entryAssemblyName = Assembly.GetEntryAssembly()?.GetName();
                    var version = entryAssemblyName?.Version?.ToString();
                    _info = $"{entryAssemblyName?.Name} | {version}";
                }
            }

            return _info;
        }

    }
}
