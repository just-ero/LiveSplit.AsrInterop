namespace LiveSplit.AsrInterop.Core;

public readonly struct ProcessHandle
{
    public static ProcessHandle Zero => new(0);

    private readonly ulong _handle;

    public ProcessHandle(ulong handle)
    {
        _handle = handle;
    }

    public bool IsValid => _handle != 0;

    public static implicit operator ulong(ProcessHandle hProcess)
    {
        return hProcess._handle;
    }

    public static implicit operator ProcessHandle(ulong handle)
    {
        return new(handle);
    }
}
