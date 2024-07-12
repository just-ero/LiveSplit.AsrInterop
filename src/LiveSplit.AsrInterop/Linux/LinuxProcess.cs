using System;

using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Linux.Native;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop.Linux;

internal sealed class LinuxProcess : Process
{
    public LinuxProcess(ProcessHandle handle)
        : base(handle) { }

    public LinuxProcess(ProcessHandle handle, string processName)
        : base(handle, processName) { }

    protected override unsafe bool GetIs64Bit()
    {
        ElfHeader elfHeader;
        if (!Read(Handle, MainModule.BaseAddress, &elfHeader, (nuint)sizeof(ElfHeader)))
        {
            throw new Exception(
                $"Could not read ELF header from module '{MainModule.ModuleName}'.");
        }

        if (elfHeader.Ident.Magic != ElfIdent.ElfSignature)
        {
            throw new BadImageFormatException(
                $"Invalid ELF header magic value in module '{MainModule.ModuleName}' " +
                $"(expected 0x{ElfIdent.ElfSignature:X4}, got 0x{elfHeader.Ident.Magic:X4}).");
        }

        return elfHeader.Ident.FileClass == ElfClass.Class64;
    }
}
