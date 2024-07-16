namespace LiveSplit.AsrInterop.Watchers;

public interface IWatcher
{
    object? Current { get; }
    object? Old { get; }

    bool Enabled { get; set; }
}

public interface IWatcher<out T> : IWatcher
{
    new T? Current { get; }
    new T? Old { get; }

    object? IWatcher.Current => Current;
    object? IWatcher.Old => Old;
}
