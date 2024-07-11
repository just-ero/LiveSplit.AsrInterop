namespace LiveSplit.AsrInterop.Core;

public readonly struct UAddress
{
    private readonly ulong _address;

    public UAddress(ulong address)
    {
        _address = address;
    }

    public bool IsValid => _address != 0;

    public static UAddress None => new(0);

    public static implicit operator ulong(UAddress address)
    {
        return address._address;
    }

    public static implicit operator UAddress(ulong address)
    {
        return new(address);
    }
}

