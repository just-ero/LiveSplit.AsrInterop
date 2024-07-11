using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public readonly record struct MemoryRange(
    UAddress BaseAddress,
    ulong RegionSize,
    MemoryRangeFlags Flags);
