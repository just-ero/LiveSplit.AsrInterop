namespace LiveSplit.AsrInterop.Core;

public readonly struct USettingsMap
{
    private readonly ulong _handle;

    public USettingsMap(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static USettingsMap None => new(0);

    public static implicit operator ulong(USettingsMap map)
    {
        return map._handle;
    }

    public static implicit operator USettingsMap(ulong handle)
    {
        return new(handle);
    }
}
