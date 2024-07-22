namespace LiveSplit.AsrInterop.Settings;

public abstract record Setting(
    string Key,
    string? Tooltip = null)
{
    public abstract void Register();
}
