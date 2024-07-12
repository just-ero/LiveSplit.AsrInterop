using System;
using System.Diagnostics.CodeAnalysis;

using LiveSplit.AsrInterop.Core;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop;

public partial class Process
{
    public static Process GetProcessById(ulong processId)
    {
        if (!Runtime.GetOs(out string? os))
        {
            throw new Exception(
                "Could not determine the operating system.");
        }

        var handle = AttachByPid(processId);
        if (!handle.IsValid)
        {
            throw new Exception(
                $"Could not attach to process with ID {processId}.");
        }

        return os switch
        {
            "linux" => new Linux.LinuxProcess(handle),
            "windows" => new Windows.WindowsProcess(handle),
            "macos" => new MacOS.MacOSProcess(handle),
            _ => throw new PlatformNotSupportedException(
                $"The operating system '{os}' is not supported.")
        };
    }

    public static bool TryGetProcessById(ulong processId, [NotNullWhen(true)] out Process? process)
    {
        if (!Runtime.GetOs(out string? os))
        {
            process = null;
            return false;
        }

        var handle = AttachByPid(processId);
        if (!handle.IsValid)
        {
            process = null;
            return false;
        }

        process = os switch
        {
            "linux" => new Linux.LinuxProcess(handle),
            "windows" => new Windows.WindowsProcess(handle),
            "macos" => new MacOS.MacOSProcess(handle),
            _ => throw new PlatformNotSupportedException(
                $"The operating system '{os}' is not supported.")
        };

        return true;
    }

    public static Process GetProcessByName(string processName)
    {
        if (!Runtime.GetOs(out string? os))
        {
            throw new Exception(
                "Could not determine the operating system.");
        }

        var handle = Attach(processName);
        if (!handle.IsValid)
        {
            throw new Exception(
                $"Could not attach to process with name '{processName}'.");
        }

        return os switch
        {
            "linux" => new Linux.LinuxProcess(handle, processName),
            "windows" => new Windows.WindowsProcess(handle, processName),
            "macos" => new MacOS.MacOSProcess(handle, processName),
            _ => throw new PlatformNotSupportedException(
                $"The operating system '{os}' is not supported.")
        };
    }

    public static bool TryGetProcessByName(string processName, [NotNullWhen(true)] out Process? process)
    {
        if (!Runtime.GetOs(out string? os))
        {
            process = null;
            return false;
        }

        var handle = Attach(processName);
        if (!handle.IsValid)
        {
            process = null;
            return false;
        }

        process = os switch
        {
            "linux" => new Linux.LinuxProcess(handle, processName),
            "windows" => new Windows.WindowsProcess(handle, processName),
            "macos" => new MacOS.MacOSProcess(handle, processName),
            _ => throw new PlatformNotSupportedException(
                $"The operating system '{os}' is not supported.")
        };

        return true;
    }

    public static Process[] GetProcessesByName(string processName)
    {
        if (!Runtime.GetOs(out string? os))
        {
            throw new Exception(
                "Could not determine the operating system.");
        }

        if (!ListByName(processName, out ulong[]? ids))
        {
            throw new Exception(
                $"Could not list processes matching name '{processName}'.");
        }

        var processes = new Process[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            var handle = AttachByPid(ids[i]);
            if (!handle.IsValid)
            {
                throw new Exception(
                    $"Could not attach to process with ID {ids[i]}.");
            }

            processes[i] = os switch
            {
                "linux" => new Linux.LinuxProcess(handle, processName),
                "windows" => new Windows.WindowsProcess(handle, processName),
                "macos" => new MacOS.MacOSProcess(handle, processName),
                _ => throw new PlatformNotSupportedException(
                    $"The operating system '{os}' is not supported.")
            };
        }

        return processes;
    }

    public static bool TryGetProcessesByName(string processName, [NotNullWhen(true)] out Process[]? processes)
    {
        if (!Runtime.GetOs(out string? os))
        {
            processes = null;
            return false;
        }

        if (!ListByName(processName, out ulong[]? ids))
        {
            processes = null;
            return false;
        }

        processes = new Process[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            var handle = AttachByPid(ids[i]);
            if (!handle.IsValid)
            {
                processes = null;
                return false;
            }

            processes[i] = os switch
            {
                "linux" => new Linux.LinuxProcess(handle, processName),
                "windows" => new Windows.WindowsProcess(handle, processName),
                "macos" => new MacOS.MacOSProcess(handle, processName),
                _ => throw new PlatformNotSupportedException(
                    $"The operating system '{os}' is not supported.")
            };
        }

        return true;
    }
}
