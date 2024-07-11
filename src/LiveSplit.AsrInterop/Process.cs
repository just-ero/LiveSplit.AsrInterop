using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using LiveSplit.AsrInterop.Core;

using static LiveSplit.AsrInterop.Core.Process;

namespace LiveSplit.AsrInterop;

public sealed class Process
{
    public Process(UHandle handle, string processName, string fileName)
    {
        Handle = handle;
        ProcessName = processName;
        FileName = fileName;
    }

    public UHandle Handle { get; }

    public string ProcessName { get; }
    public string FileName { get; }

    public bool HasExited => IsOpen(Handle);

    public static Process GetProcessById(ulong processId)
    {
        if (AttachByPid(processId) is not { IsValid: true } handle)
        {
            string msg = $"No process with the specified ID '{processId}' could be found.";
            throw new InvalidOperationException(msg);
        }

        if (!GetPath(handle, out string? fileName))
        {
            string msg = $"Failed to get process path for process with ID '{processId}'.";
            throw new InvalidOperationException(msg);
        }

        return new(handle, Path.GetFileNameWithoutExtension(fileName), fileName);
    }

    public static bool TryGetProcessById(ulong processId, [NotNullWhen(true)] out Process? process)
    {
        if (AttachByPid(processId) is not { IsValid: true } handle)
        {
            process = null;
            return false;
        }

        if (!GetPath(handle, out string? fileName))
        {
            process = null;
            return false;
        }

        process = new(handle, Path.GetFileNameWithoutExtension(fileName), fileName);
        return true;
    }

    public static Process GetProcessByName(string processName)
    {
        if (Attach(processName) is not { IsValid: true } handle)
        {
            string msg = $"No processes with the specified name '{processName}' could be found.";
            throw new InvalidOperationException(msg);
        }

        if (!GetPath(handle, out string? fileName))
        {
            string msg = $"Failed to get process path for process with name '{processName}'.";
            throw new InvalidOperationException(msg);
        }

        return new(handle, processName, fileName);
    }

    public static bool TryGetProcessByName(string processName, [NotNullWhen(true)] out Process? process)
    {
        if (Attach(processName) is not { IsValid: true } handle)
        {
            process = null;
            return false;
        }

        if (!GetPath(handle, out string? fileName))
        {
            process = null;
            return false;
        }

        process = new(handle, processName, fileName);
        return true;
    }

    public static Process[] GetProcessesByName(string processName)
    {
        if (!ListByName(processName, out ulong[]? ids))
        {
            string msg = $"No processes with the specified name '{processName}' could be found.";
            throw new InvalidOperationException(msg);
        }

        Process[] processes = new Process[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            UHandle handle = AttachByPid(ids[i]);
            if (!handle.IsValid)
            {
                string msg = $"Failed to attach to process with ID '{ids[i]}'.";
                throw new InvalidOperationException(msg);
            }

            if (!GetPath(ids[i], out string? fileName))
            {
                string msg = $"Failed to get process path for process with ID '{ids[i]}'.";
                throw new InvalidOperationException(msg);
            }

            processes[i] = new(ids[i], processName, fileName);
        }

        return processes;
    }

    public static bool TryGetProcessesByName(string processName, [NotNullWhen(true)] out Process[]? processes)
    {
        if (!ListByName(processName, out ulong[]? ids))
        {
            processes = null;
            return false;
        }

        processes = new Process[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            UHandle handle = AttachByPid(ids[i]);
            if (!handle.IsValid)
            {
                processes = null;
                return false;
            }

            if (!GetPath(ids[i], out string? fileName))
            {
                processes = null;
                return false;
            }

            processes[i] = new(ids[i], processName, fileName);
        }

        return true;
    }
}
