using System;
using System.IO;

using LiveSplit.AsrInterop.Core;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop;

public abstract partial class Process
{
    private string? _processName;
    private Module? _mainModule;
    private bool? _is64Bit;

    protected Process(ProcessHandle handle)
    {
        Handle = handle;
    }

    protected Process(ProcessHandle handle, string processName)
        : this(handle)
    {
        _processName = processName;
    }

    public ProcessHandle Handle { get; }

    public string ProcessName => _processName ??= MainModule.ModuleName;
    public Module MainModule => _mainModule ??= GetMainModule(Handle);
    public bool Is64Bit => _is64Bit ??= GetIs64Bit();

    public bool HasExited => !IsOpen(Handle);

    protected abstract bool GetIs64Bit();

    private static Module GetMainModule(ProcessHandle handle)
    {
        if (!GetPath(handle, out string? path))
        {
            throw new Exception(
                "Could not get executable file path.");
        }

        var mmName = Path.GetFileName(path);

        var mmBase = GetModuleAddress(handle, mmName);
        if (!mmBase.IsValid)
        {
            throw new Exception(
                $"Could not get main module base address for '{mmName}'.");
        }

        var mmSize = GetModuleSize(handle, mmName);
        if (mmSize == 0)
        {
            throw new Exception(
                $"Could not get main module size for '{mmName}'.");
        }

        return new(mmName, mmBase, mmSize, path);
    }
}
