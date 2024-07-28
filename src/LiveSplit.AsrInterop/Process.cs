using System;
using System.IO;

namespace LiveSplit.AsrInterop;

public sealed partial class Process
{
    private string? _processName;
    private Module? _mainModule;
    private bool? _is64Bit;

    public Process(Core.Process owner)
    {
        Owner = owner;
    }

    public Core.Process Owner { get; }

    public string ProcessName => _processName ??= MainModule.ModuleName;
    public Module MainModule => _mainModule ??= MainModuleInternal(Owner);
    public bool Is64Bit => _is64Bit ??= Is64BitInternal(Owner, MainModule);

    private static Module MainModuleInternal(Core.Process process)
    {
        if (!process.TryGetPath(out string? path))
        {
            throw new Exception(
                "Could not get executable file path.");
        }

        var mmName = Path.GetFileName(path);

        var mmBase = process.GetModuleAddress(mmName);
        if (!mmBase.IsValid)
        {
            throw new Exception(
                $"Could not get main module base address for '{mmName}'.");
        }

        var mmSize = process.GetModuleSize(mmName);
        if (mmSize == 0)
        {
            throw new Exception(
                $"Could not get main module size for '{mmName}'.");
        }

        return new(mmName, mmBase, mmSize, path);
    }
}
