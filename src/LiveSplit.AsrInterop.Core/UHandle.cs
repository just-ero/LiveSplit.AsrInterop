namespace LiveSplit.AsrInterop.Core;

public readonly struct UHandle
{
    private readonly ulong _handle;

    public UHandle(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static UHandle None => new(0);

    public static implicit operator ulong(UHandle hProcess)
    {
        return hProcess._handle;
    }

    public static implicit operator UHandle(ulong handle)
    {
        return new(handle);
    }
}
