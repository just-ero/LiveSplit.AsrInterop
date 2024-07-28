using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Extensions;

namespace LiveSplit.AsrInterop.Watchers;

public sealed class Watcher<T> : WatcherBase<T>
    where T : unmanaged
{
    private readonly Process _process;

    public Watcher(Process process, Address startAddress, params uint[] offsets)
        : base(startAddress, offsets)
    {
        _process = process;
    }

    public Watcher(Process process, TickCounter tickCounter, Address startAddress, params uint[] offsets)
        : base(tickCounter, startAddress, offsets)
    {
        _process = process;
    }

    protected override T ReadValue(Address startAddress, uint[] offsets)
    {
        return _process.Read<T>(startAddress, offsets);
    }

    public static Watcher<T> Create(Address startAddress, params uint[] offsets)
    {
        return new(startAddress, offsets);
    }
}
