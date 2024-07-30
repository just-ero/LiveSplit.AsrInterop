using System.Text;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Watchers;

public abstract class WatcherBase<T> : IWatcher<T>
{
    private readonly Address _startAddress;
    private readonly uint[] _offsets;

    // private ulong _tick;
    private T? _current;
    private T? _old;

    protected WatcherBase(Address startAddress, params uint[] offsets)
    {
        _startAddress = startAddress;
        _offsets = offsets;
    }

    public T? Current
    {
        get
        {
            Update();

            return _current;
        }
    }

    public T? Old
    {
        get
        {
            Update();

            return _old;
        }
    }

    public bool Enabled { get; set; } = true;

    protected abstract T? ReadValue(Address startAddress, uint[] offsets);

    private void Update()
    {
        if (!Enabled)
        {
            return;
        }

        _old = _current;
        _current = ReadValue(_startAddress, _offsets);
    }

    public sealed override string ToString()
    {
        StringBuilder path = new($"0x{(ulong)_startAddress:X}");
        foreach (uint offset in _offsets)
        {
            path.Append($", 0x{offset:X}");
        }

        return ToString(path.ToString());
    }

    protected abstract string ToString(string path);
}
