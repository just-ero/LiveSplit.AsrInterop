using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public sealed record Module(
    string ModuleName,
    Address BaseAddress,
    ulong Size,
    string FileName);
