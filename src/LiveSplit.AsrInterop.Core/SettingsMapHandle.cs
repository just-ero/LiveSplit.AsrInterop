namespace LiveSplit.AsrInterop.Core;

public readonly struct SettingsMapHandle
{
    public static SettingsMapHandle Zero => new(0);

    private readonly ulong _handle;

    public SettingsMapHandle(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static implicit operator ulong(SettingsMapHandle map)
    {
        return map._handle;
    }

    public static implicit operator SettingsMapHandle(ulong handle)
    {
        return new(handle);
    }
}
