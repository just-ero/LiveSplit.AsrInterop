using System;
using System.Diagnostics.CodeAnalysis;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

public static partial class ProcessExtensions
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
        Address baseAddress = process.Owner.GetModuleAddress(moduleName);
        if (!baseAddress.IsValid)
        {
            throw new InvalidOperationException(
                $"Could not retrieve base address of module '{moduleName}'.");
        }

        ulong memorySize = process.Owner.GetModuleSize(moduleName);
        if (memorySize == 0)
        {
            throw new InvalidOperationException(
                $"Could not retrieve memory size of module '{moduleName}'.");
        }

        if (!process.Owner.TryGetModulePath(moduleName, out string? modulePath))
        {
            throw new InvalidOperationException(
                $"Could not retrieve path of module '{moduleName}'.");
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
        Address baseAddress = process.Owner.GetModuleAddress(moduleName);
        if (!baseAddress.IsValid)
        {
            module = default;
            return false;
        }

        ulong memorySize = process.Owner.GetModuleSize(moduleName);
        if (memorySize == 0)
        {
            module = default;
            return false;
        }

        if (!process.Owner.TryGetModulePath(moduleName, out string? modulePath))
        {
            module = default;
            return false;
        }

        module = new(moduleName, baseAddress, memorySize, modulePath);
        return true;
    }

    public static MemoryRange[] GetMemoryRanges(this Process process)
    {
        ulong count = process.Owner.MemoryRangeCount;
        if (count == 0)
        {
            string msg = "Failed to get memory range count.";
            throw new InvalidOperationException(msg);
        }

        MemoryRange[] ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            Address baseAddress = process.Owner.GetMemoryRangeAddress(i);
            if (!baseAddress.IsValid)
            {
                string msg = $"Failed to get memory range address for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            ulong regionSize = process.Owner.GetMemoryRangeSize(i);
            if (regionSize == 0)
            {
                string msg = $"Failed to get memory range size for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            MemoryRangeFlags flags = process.Owner.GetMemoryRangeFlags(i);
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
        ulong count = process.Owner.MemoryRangeCount;
        if (count == 0)
        {
            ranges = default;
            return false;
        }

        ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            Address baseAddress = process.Owner.GetMemoryRangeAddress(i);
            if (!baseAddress.IsValid)
            {
                ranges = default;
                return false;
            }

            ulong regionSize = process.Owner.GetMemoryRangeSize(i);
            if (regionSize == 0)
            {
                ranges = default;
                return false;
            }

            MemoryRangeFlags flags = process.Owner.GetMemoryRangeFlags(i);
            if (flags == MemoryRangeFlags.None)
            {
                ranges = default;
                return false;
            }

            ranges[i] = new(baseAddress, regionSize, flags);
        }

        return true;
    }
}
