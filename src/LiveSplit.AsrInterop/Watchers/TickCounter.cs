namespace LiveSplit.AsrInterop.Watchers;

public struct TickCounter
{
    public ulong Ticks { get; private set; } = 1;

    public TickCounter() { }

    public TickCounter(ulong ticks)
    {
        Ticks = ticks;
    }

    public static TickCounter operator ++(TickCounter counter)
    {
        counter.Ticks++;
        return counter;
    }
}
