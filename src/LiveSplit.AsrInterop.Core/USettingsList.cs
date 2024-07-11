namespace LiveSplit.AsrInterop.Core;

public readonly struct USettingsList
{
    private readonly ulong _handle;

    public USettingsList(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static USettingsList None => new(0);

    public static implicit operator ulong(USettingsList list)
    {
        return list._handle;
    }

    public static implicit operator USettingsList(ulong handle)
    {
        return new(handle);
    }
}
