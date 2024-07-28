using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Watchers;

namespace LiveSplit.AsrInterop.Extensions;

public static class WatcherExtensions
{
    public static IWatcher<T> Enabled<T>(this IWatcher<T> watcher, bool enabled)
    {
        watcher.Enabled = enabled;
        return watcher;
    }

    // [OverloadResolutionPriority(1)]
    public static IWatcher<T> Watch<T>(this Process process, long startOffset, params uint[] offsets)
        where T : unmanaged
    {
        return Watch<T>(process, process.MainModule, startOffset, offsets);
    }

    public static IWatcher<T> Watch<T>(this Process process, string moduleName, long startOffset, params uint[] offsets)
        where T : unmanaged
    {
        return Watch<T>(process, process.GetModule(moduleName), startOffset, offsets);
    }

    public static IWatcher<T> Watch<T>(this Process process, Module module, long startOffset, params uint[] offsets)
        where T : unmanaged
    {
        return Watch<T>(process, module.BaseAddress + (ulong)startOffset, offsets);
    }

    public static IWatcher<T> Watch<T>(this Process process, Address startAddress, params uint[] offsets)
        where T : unmanaged
    {
        return new Watcher<T>(process, startAddress, offsets);
    }
}
