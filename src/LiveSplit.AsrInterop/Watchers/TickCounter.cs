namespace LiveSplit.AsrInterop.Watchers;

public sealed class TickCounter
{
    public TickCounter() { }

    public TickCounter(ulong ticks)
    {
        Ticks = ticks;
    }

    public ulong Ticks { get; private set; } = 1;

    public static TickCounter operator ++(TickCounter counter)
    {
        counter.Ticks++;
        return counter;
    }
}
