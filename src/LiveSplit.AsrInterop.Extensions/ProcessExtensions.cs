using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using LiveSplit.AsrInterop.Core;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop.Extensions;

public static class ProcessExtensions
{
    /// <summary>
    ///     Retrieves the module with the specified name from the specified process.
    /// </summary>
    /// <param name="process">
    ///     The process from which to retrieve the module.
    /// </param>
    /// <param name="moduleName">
    ///     The name of the module to retrieve.
    /// </param>
    /// <returns>
    ///     The module with the specified name.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the base address, memory size, or path of the module could not be retrieved.
    /// </exception>
    public static Module GetModule(this Process process, string moduleName)
    {
        UAddress baseAddress = GetModuleAddress(process.Handle, moduleName);
        if (!baseAddress.IsValid)
        {
            string msg = $"Could not retrieve base address of module '{moduleName}'.";
            throw new InvalidOperationException(msg);
        }

        ulong memorySize = GetModuleSize(process.Handle, moduleName);
        if (memorySize == 0)
        {
            string msg = $"Could not retrieve memory size of module '{moduleName}'.";
            throw new InvalidOperationException(msg);
        }

        if (!GetModulePath(process.Handle, moduleName, out string? modulePath))
        {
            string msg = $"Could not retrieve path of module '{moduleName}'.";
            throw new InvalidOperationException(msg);
        }

        return new(moduleName, baseAddress, memorySize, modulePath);
    }

    /// <summary>
    ///     Attempts to retrieve the module with the specified name from the specified process.
    /// </summary>
    /// <param name="process">
    ///     The process from which to retrieve the module.
    /// </param>
    /// <param name="moduleName">
    ///     The name of the module to retrieve.
    /// </param>
    /// <param name="module">
    ///     The module with the specified name when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the module was successfully retrieved;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetModule(this Process process, string moduleName, [NotNullWhen(true)] out Module? module)
    {
        UAddress baseAddress = GetModuleAddress(process.Handle, moduleName);
        if (!baseAddress.IsValid)
        {
            module = default;
            return false;
        }

        ulong memorySize = GetModuleSize(process.Handle, moduleName);
        if (memorySize == 0)
        {
            module = default;
            return false;
        }

        if (!GetModulePath(process.Handle, moduleName, out string? modulePath))
        {
            module = default;
            return false;
        }

        module = new(moduleName, baseAddress, memorySize, modulePath);
        return true;
    }

    public static MemoryRange[] GetMemoryRanges(this Process process)
    {
        ulong count = GetMemoryRangeCount(process.Handle);
        if (count == 0)
        {
            string msg = "Failed to get memory range count.";
            throw new InvalidOperationException(msg);
        }

        MemoryRange[] ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            UAddress baseAddress = GetMemoryRangeAddress(process.Handle, i);
            if (!baseAddress.IsValid)
            {
                string msg = $"Failed to get memory range address for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            ulong regionSize = GetMemoryRangeSize(process.Handle, i);
            if (regionSize == 0)
            {
                string msg = $"Failed to get memory range size for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            MemoryRangeFlags flags = GetMemoryRangeFlags(process.Handle, i);
            if (flags == MemoryRangeFlags.None)
            {
                string msg = $"Failed to get memory range flags for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            ranges[i] = new(baseAddress, regionSize, flags);
        }

        return ranges;
    }

    public static bool TryGetMemoryRanges(this Process process, [NotNullWhen(true)] out MemoryRange[]? ranges)
    {
        ulong count = GetMemoryRangeCount(process.Handle);
        if (count == 0)
        {
            ranges = default;
            return false;
        }

        ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            UAddress baseAddress = GetMemoryRangeAddress(process.Handle, i);
            if (!baseAddress.IsValid)
            {
                ranges = default;
                return false;
            }

            ulong regionSize = GetMemoryRangeSize(process.Handle, i);
            if (regionSize == 0)
            {
                ranges = default;
                return false;
            }

            MemoryRangeFlags flags = GetMemoryRangeFlags(process.Handle, i);
            if (flags == MemoryRangeFlags.None)
            {
                ranges = default;
                return false;
            }

            ranges[i] = new(baseAddress, regionSize, flags);
        }

        return true;
    }

    public static unsafe UAddress Deref(this Process process, UAddress address, params ReadOnlySpan<uint> offsets)
    {
        UAddress deref = address;
        foreach (uint offset in offsets)
        {
            if (!Core.Process.Read(process.Handle, deref, &deref, process.GetNativeSizeOf<nuint>()))
            {
                return UAddress.Zero;
            }

            deref += offset;
        }

        return deref;
    }

    public static unsafe T Read<T>(this Process process, UAddress address, params ReadOnlySpan<uint> offsets)
        where T : unmanaged
    {
        var deref = process.Deref(address, offsets);

        T value;
        if (Core.Process.Read(process.Handle, deref, &value, process.GetNativeSizeOf<T>()))
        {
            return value;
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsNativeType<T>()
    {
        return typeof(T) == typeof(nint) || typeof(T) == typeof(nuint) || typeof(T) == typeof(UAddress);
    }

    private static unsafe uint GetNativeSizeOf<T>(this Process process)
        where T : unmanaged
    {
        return IsNativeType<T>() ? (process.Is64Bit ? 0x8u : 0x4u) : (uint)sizeof(T);
    }
}
