using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public readonly record struct Module(
    Address BaseAddress,
    ulong Size,
    string FileName);
