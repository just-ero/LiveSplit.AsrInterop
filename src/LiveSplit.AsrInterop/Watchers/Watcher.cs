using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Extensions;

namespace LiveSplit.AsrInterop.Watchers;

public sealed class Watcher<T> : WatcherBase<T>
    where T : unmanaged
{
    private readonly ExternalProcess _process;

    public Watcher(ExternalProcess process, Address startAddress, params uint[] offsets)
        : base(startAddress, offsets)
    {
        _process = process;
    }

    protected override T ReadValue(Address startAddress, uint[] offsets)
    {
        return _process.Read<T>(startAddress, offsets);
    }

    protected override string ToString(string path)
    {
        return $"Watcher<{typeof(T)}>({path})";
    }
}
