using System;

using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Native.Linux;
using LiveSplit.AsrInterop.Native.Windows;

namespace LiveSplit.AsrInterop;

public partial class ExternalProcess
{
    private static bool Is64BitInternal(Core.Process process, Module module)
    {
        if (!Runtime.TryGetOs(out string? os))
        {
            throw new Exception(
                "Could not determine the operating system.");
        }

        return os switch
        {
            "linux" => Is64BitLinux(process, module),
            "macos" => Is64BitMacOS(process, module),
            "windows" => Is64BitWindows(process, module),
            _ => throw new PlatformNotSupportedException(
                $"The operating system '{os}' is not supported.")
        };
    }

    private static unsafe bool Is64BitLinux(Core.Process process, Module module)
    {
        ElfHeader elfHeader;
        if (!process.TryRead(module.BaseAddress, &elfHeader, (nuint)sizeof(ElfHeader)))
        {
            throw new Exception(
                $"Could not read ELF header from module '{module.ModuleName}'.");
        }

        if (elfHeader.Ident.Magic != ElfIdent.ElfSignature)
        {
            throw new BadImageFormatException(
                $"Invalid ELF header magic value in module '{module.ModuleName}' " +
                $"(expected 0x{ElfIdent.ElfSignature:X4}, got 0x{elfHeader.Ident.Magic:X4}).");
        }

        return elfHeader.Ident.FileClass == ElfClass.Class64;
    }

    private static bool Is64BitMacOS(Core.Process process, Module module)
    {
        return true;
    }

    private static unsafe bool Is64BitWindows(Core.Process process, Module module)
    {
        ImageDosHeader dosHeader;
        if (!process.TryRead(module.BaseAddress, &dosHeader, (nuint)sizeof(ImageDosHeader)))
        {
            throw new Exception(
                $"Could not read DOS header from module '{module.ModuleName}'.");
        }

        if (dosHeader.Magic != ImageDosHeader.ImageDosSignature)
        {
            throw new BadImageFormatException(
                $"Invalid DOS header magic value in module '{module.ModuleName}' " +
                $"(expected 0x{ImageDosHeader.ImageDosSignature:X4}, got 0x{dosHeader.Magic:X4}).");
        }

        ImageNtHeaders ntHeaders;
        if (!process.TryRead(module.BaseAddress + (uint)dosHeader.NtHeaderAddress, &ntHeaders, (nuint)sizeof(ImageNtHeaders)))
        {
            throw new Exception(
                $"Could not read NT headers from module '{module.ModuleName}'.");
        }

        if (ntHeaders.Signature != ImageNtHeaders.ImageNtSignature)
        {
            throw new BadImageFormatException(
                $"Invalid NT headers signature in module '{module.ModuleName}' " +
                $"(expected 0x{ImageNtHeaders.ImageNtSignature:X4}, got 0x{ntHeaders.Signature:X4}).");
        }

        return ntHeaders.FileHeader.Machine
            is ImageFileMachine.IA64
            or ImageFileMachine.AMD64
            or ImageFileMachine.ARM64;
    }
}
