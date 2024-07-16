using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

internal static class MemoryHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNativeType<T>()
    {
        return typeof(T) == typeof(Address);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe uint GetNativeSizeOf<T>(Process process)
        where T : unmanaged
    {
        return IsNativeType<T>()
            ? (process.Is64Bit ? 0x8u : 0x4u)
            : (uint)sizeof(T);
    }
}
