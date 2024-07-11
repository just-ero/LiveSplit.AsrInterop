using System;

namespace LiveSplit.AsrInterop.Core;

[Flags]
public enum MemoryRangeFlags : ulong
{
    None = 0,

    Read = 1 << 1,
    Write = 1 << 2,
    Execute = 1 << 3,

    HasPath = 1 << 4
}
