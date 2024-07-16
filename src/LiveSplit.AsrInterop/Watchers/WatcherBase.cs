using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Watchers;

public abstract class WatcherBase<T> : IWatcher<T>
{
    private readonly TickCounter? _tickCounter;
    private readonly Address _startAddress;
    private readonly uint[] _offsets;

    private ulong _tick;
    private T? _current;
    private T? _old;

    public WatcherBase(Address startAddress, params uint[] offsets)
    {
        _startAddress = startAddress;
        _offsets = offsets;
    }

    public WatcherBase(TickCounter tickCounter, Address startAddress, params uint[] offsets)
    {
        _tickCounter = tickCounter;
        _startAddress = startAddress;
        _offsets = offsets;
    }

    public T? Current
    {
        get
        {
            if (_tickCounter is { Ticks: ulong tick })
            {
                if (_tick != tick)
                {
                    _tick = tick;
                    Update();
                }
            }
            else
            {
                Update();
            }

            return _current;
        }
    }

    public T? Old
    {
        get
        {
            if (_tickCounter is { Ticks: ulong tick })
            {
                if (_tick != tick)
                {
                    _tick = tick;
                    Update();
                }
            }
            else
            {
                Update();
            }

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
}
