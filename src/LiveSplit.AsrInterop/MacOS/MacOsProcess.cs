using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.MacOS;

internal sealed class MacOSProcess : Process
{
    public MacOSProcess(ProcessHandle handle)
        : base(handle) { }

    public MacOSProcess(ProcessHandle handle, string processName)
        : base(handle, processName) { }

    protected override bool GetIs64Bit()
    {
        return true;
    }
}
