using System;
using System.Diagnostics.CodeAnalysis;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

/// <summary>
///     Provides extension methods for <see cref="Process"/>.
/// </summary>
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
        Address baseAddress = process.GetModuleAddress(moduleName);
        if (!baseAddress.IsValid)
        {
            throw new InvalidOperationException(
                $"Could not retrieve base address of module '{moduleName}'.");
        }

        ulong memorySize = process.GetModuleSize(moduleName);
        if (memorySize == 0)
        {
            throw new InvalidOperationException(
                $"Could not retrieve memory size of module '{moduleName}'.");
        }

        if (!process.TryGetModulePath(moduleName, out string? modulePath))
        {
            throw new InvalidOperationException(
                $"Could not retrieve path of module '{moduleName}'.");
        }

        return new(baseAddress, memorySize, modulePath);
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
    ///     otherwise, an invalid module.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the module was successfully retrieved;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetModule(this Process process, string moduleName, out Module module)
    {
        Address baseAddress = process.GetModuleAddress(moduleName);
        if (!baseAddress.IsValid)
        {
            module = default;
            return false;
        }

        ulong memorySize = process.GetModuleSize(moduleName);
        if (memorySize == 0)
        {
            module = default;
            return false;
        }

        if (!process.TryGetModulePath(moduleName, out string? modulePath))
        {
            module = default;
            return false;
        }

        module = new(baseAddress, memorySize, modulePath);
        return true;
    }

    /// <summary>
    ///     Retrieves the memory ranges in the specified process.
    /// </summary>
    /// <param name="process">
    ///     The process from which to retrieve the memory ranges.
    /// </param>
    /// <returns>
    ///     The memory ranges in the specified process.
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static MemoryRange[] GetMemoryRanges(this Process process)
    {
        ulong count = process.MemoryRangeCount;
        if (count == 0)
        {
            return [];
        }

        MemoryRange[] ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            Address baseAddress = process.GetMemoryRangeAddress(i);
            if (!baseAddress.IsValid)
            {
                string msg = $"Failed to get memory range address for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            ulong regionSize = process.GetMemoryRangeSize(i);
            if (regionSize == 0)
            {
                string msg = $"Failed to get memory range size for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            MemoryRangeFlags flags = process.GetMemoryRangeFlags(i);
            if (flags == MemoryRangeFlags.None)
            {
                string msg = $"Failed to get memory range flags for range at index '{i}'.";
                throw new InvalidOperationException(msg);
            }

            ranges[i] = new(baseAddress, regionSize, flags);
        }

        return ranges;
    }

    /// <summary>
    ///     Attempts to retrieve the memory ranges in the specified process.
    /// </summary>
    /// <param name="process">
    ///     The process from which to retrieve the memory ranges.
    /// </param>
    /// <param name="ranges">
    ///     The memory ranges in the specified process when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the memory ranges were successfully retrieved;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetMemoryRanges(this Process process, [NotNullWhen(true)] out MemoryRange[]? ranges)
    {
        ulong count = process.MemoryRangeCount;
        if (count == 0)
        {
            ranges = [];
            return true;
        }

        ranges = new MemoryRange[count];
        for (ulong i = 0; i < count; i++)
        {
            Address baseAddress = process.GetMemoryRangeAddress(i);
            if (!baseAddress.IsValid)
            {
                ranges = default;
                return false;
            }

            ulong regionSize = process.GetMemoryRangeSize(i);
            if (regionSize == 0)
            {
                ranges = default;
                return false;
            }

            MemoryRangeFlags flags = process.GetMemoryRangeFlags(i);
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
