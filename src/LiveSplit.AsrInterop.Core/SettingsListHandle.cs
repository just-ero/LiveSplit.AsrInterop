namespace LiveSplit.AsrInterop.Core;

public readonly struct SettingsListHandle
{
    public static SettingsListHandle Zero => new(0);

    private readonly ulong _handle;

    public SettingsListHandle(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static implicit operator ulong(SettingsListHandle list)
    {
        return list._handle;
    }

    public static implicit operator SettingsListHandle(ulong handle)
    {
        return new(handle);
    }
}
