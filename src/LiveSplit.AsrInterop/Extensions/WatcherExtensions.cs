using LiveSplit.AsrInterop.Watchers;

namespace LiveSplit.AsrInterop.Extensions;

public static class WatcherExtensions
{
    public static IWatcher<T> Enabled<T>(this IWatcher<T> watcher, bool enabled)
    {
        watcher.Enabled = enabled;
        return watcher;
    }
}
