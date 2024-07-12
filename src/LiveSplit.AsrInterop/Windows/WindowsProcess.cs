using System;

using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Windows.Native;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop.Windows;

internal sealed class WindowsProcess : Process
{
    public WindowsProcess(ProcessHandle handle)
        : base(handle) { }

    public WindowsProcess(ProcessHandle handle, string processName)
        : base(handle, processName) { }

    protected override unsafe bool GetIs64Bit()
    {
        ImageDosHeader dosHeader;
        if (!Read(Handle, MainModule.BaseAddress, &dosHeader, (nuint)sizeof(ImageDosHeader)))
        {
            throw new Exception(
                $"Could not read DOS header from module '{MainModule.ModuleName}'.");
        }

        if (dosHeader.Magic != ImageDosHeader.ImageDosSignature)
        {
            throw new BadImageFormatException(
                $"Invalid DOS header magic value in module '{MainModule.ModuleName}' " +
                $"(expected 0x{ImageDosHeader.ImageDosSignature:X4}, got 0x{dosHeader.Magic:X4}).");
        }

        ImageNtHeaders ntHeaders;
        if (!Read(Handle, MainModule.BaseAddress + (uint)dosHeader.NtHeaderAddress, &ntHeaders, (nuint)sizeof(ImageNtHeaders)))
        {
            throw new Exception(
                $"Could not read NT headers from module '{MainModule.ModuleName}'.");
        }

        if (ntHeaders.Signature != ImageNtHeaders.ImageNtSignature)
        {
            throw new BadImageFormatException(
                $"Invalid NT headers signature in module '{MainModule.ModuleName}' " +
                $"(expected 0x{ImageNtHeaders.ImageNtSignature:X4}, got 0x{ntHeaders.Signature:X4}).");
        }

        return ntHeaders.FileHeader.Machine
            is ImageFileMachine.IA64
            or ImageFileMachine.AMD64
            or ImageFileMachine.ARM64;
    }
}
