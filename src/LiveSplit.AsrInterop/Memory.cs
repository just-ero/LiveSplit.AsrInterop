using System;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public static class Memory
{
    public static unsafe UAddress Deref(ProcessHandle handle, UAddress startAddress, params ReadOnlySpan<uint> offsets)
    {
        UAddress deref = startAddress;

        foreach (uint offset in offsets)
        {
            if (!Core.Process.Read(handle, deref, &deref, (nuint)sizeof(UAddress)))
            {
                throw new Exception(
                    $"Failed to read memory at address 0x{(ulong)deref:X}.");
            }

            deref += offset;
        }

        return deref;
    }

    public static unsafe T Read<T>(ProcessHandle handle, UAddress address, params ReadOnlySpan<uint> offsets)
        where T : unmanaged
    {
        UAddress deref = Deref(handle, address, offsets);

        T value;
        if (!Core.Process.Read(handle, deref, &value, (nuint)sizeof(T)))
        {
            throw new Exception(
                $"Failed to read memory at address 0x{(ulong)deref:X}.");
        }

        return value;
    }
}
