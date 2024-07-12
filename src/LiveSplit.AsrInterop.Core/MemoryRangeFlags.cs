using System;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents the enabled flags for the memory range.
/// </summary>
[Flags]
public enum MemoryRangeFlags : ulong
{
    /// <summary>
    ///     No flags.
    /// </summary>
    /// <remarks>
    ///     This indicates an invalid memory range.
    /// </remarks>
    None = 0,

    /// <summary>
    ///     The memory range's read access is enabled.
    /// </summary>
    Read = 1 << 1,

    /// <summary>
    ///     The memory range's write access is enabled.
    /// </summary>
    Write = 1 << 2,

    /// <summary>
    ///     The memory range's execute access is enabled.
    /// </summary>
    Execute = 1 << 3,

    /// <summary>
    ///     The memory range has a file path.
    /// </summary>
    HasPath = 1 << 4
}
