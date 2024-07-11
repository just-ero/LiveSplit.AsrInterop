namespace LiveSplit.AsrInterop.Core;

public readonly struct USettingValue
{
    private readonly ulong _handle;

    public USettingValue(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static USettingValue None => new(0);

    public static implicit operator ulong(USettingValue value)
    {
        return value._handle;
    }

    public static implicit operator USettingValue(ulong handle)
    {
        return new(handle);
    }
}
