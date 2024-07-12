using System.Numerics;

namespace LiveSplit.AsrInterop.Core;

public readonly struct UAddress
    : IEqualityOperators<UAddress, UAddress, bool>
    , IAdditionOperators<UAddress, UAddress, UAddress>
    , ISubtractionOperators<UAddress, UAddress, UAddress>
{
    public static UAddress Zero => new(0);

    private readonly ulong _address;

    public UAddress(ulong address)
    {
        _address = address;
    }

    public bool IsValid => _address != 0;

    public override bool Equals(object? obj)
    {
        return obj is UAddress address && Equals(address);
    }

    public override int GetHashCode()
    {
        return _address.GetHashCode();
    }

    public static implicit operator ulong(UAddress address)
    {
        return address._address;
    }

    public static implicit operator UAddress(ulong address)
    {
        return new(address);
    }

    public static bool operator ==(UAddress left, UAddress right)
    {
        return left._address == right._address;
    }

    public static bool operator !=(UAddress left, UAddress right)
    {
        return left._address != right._address;
    }

    public static UAddress operator +(UAddress left, UAddress right)
    {
        return new(left._address + right._address);
    }

    public static UAddress operator -(UAddress left, UAddress right)
    {
        return new(left._address - right._address);
    }
}
