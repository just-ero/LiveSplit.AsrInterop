using System.Numerics;

namespace LiveSplit.AsrInterop.Core;

public readonly struct Address
    : IEqualityOperators<Address, Address, bool>
    , IAdditionOperators<Address, Address, Address>
    , ISubtractionOperators<Address, Address, Address>
{
    public static Address Zero => new(0);

    private readonly ulong _address;

    public Address(ulong address)
    {
        _address = address;
    }

    public bool IsValid => _address != 0;

    public override bool Equals(object? obj)
    {
        return obj is Address address && Equals(address);
    }

    public override int GetHashCode()
    {
        return _address.GetHashCode();
    }

    public static implicit operator ulong(Address address)
    {
        return address._address;
    }

    public static implicit operator Address(ulong address)
    {
        return new(address);
    }

    public static bool operator ==(Address left, Address right)
    {
        return left._address == right._address;
    }

    public static bool operator !=(Address left, Address right)
    {
        return left._address != right._address;
    }

    public static Address operator +(Address left, Address right)
    {
        return new(left._address + right._address);
    }

    public static Address operator -(Address left, Address right)
    {
        return new(left._address - right._address);
    }
}
