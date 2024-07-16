using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public readonly record struct MemoryRange(
    Address BaseAddress,
    ulong RegionSize,
    MemoryRangeFlags Flags);
