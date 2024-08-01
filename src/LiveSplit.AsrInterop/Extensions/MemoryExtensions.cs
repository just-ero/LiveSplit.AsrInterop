using System;
using System.Numerics;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

public class PointerSize<T>
    where T : unmanaged, IBitwiseOperators<T, T, T>
{
    private readonly T _mask;

    public PointerSize(T mask)
    {
        _mask = mask;
    }

    public TOther GetPointer<TOther>(TOther address)
        where TOther : unmanaged, IBitwiseOperators<TOther, T, TOther>
    {
        return address & _mask;
    }
}

public static unsafe class MemoryExtensions
{
    public static Address Deref(this Process process, PointerSize<ulong> size, Address startAddress, params ReadOnlySpan<int> offsets)
    {
        var deref = startAddress;

        foreach (var offset in offsets)
        {
            if (!process.TryRead(deref, &deref, 4))
            {
                return Address.Zero;
            }

            deref += (Address)offset;
        }

        return deref;
    }
}
