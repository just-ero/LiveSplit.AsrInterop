using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public sealed record Module(
    string ModuleName,
    UAddress BaseAddress,
    ulong Size,
    string FileName);
