namespace LiveSplit.AsrInterop.Core;

public readonly struct SettingValueHandle
{
    public static SettingValueHandle Zero => new(0);

    private readonly ulong _handle;

    public SettingValueHandle(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static implicit operator ulong(SettingValueHandle value)
    {
        return value._handle;
    }

    public static implicit operator SettingValueHandle(ulong handle)
    {
        return new(handle);
    }
}
