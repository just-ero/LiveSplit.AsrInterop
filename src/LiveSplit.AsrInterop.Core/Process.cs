using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents a process.
/// </summary>
public readonly struct Process
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Process"/> struct
    ///     with the specified process handle.
    /// </summary>
    /// <param name="handle">
    ///     The handle of the process.
    /// </param>
    public Process(ulong handle)
    {
        Handle = handle;
    }

    /// <summary>
    ///     Gets the handle of the process.
    /// </summary>
    public ulong Handle { get; }

    /// <summary>
    ///     Gets a value indicating whether the process is valid.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the process' handle is not zero;
    ///     otherwise, <see langword="false"/>.
    /// </value>
    public bool IsValid => Handle != 0;

    /// <summary>
    ///     Gets a value indicating whether the process is open.
    /// </summary>
    public bool IsOpen => sys.process_is_open(Handle) != 0;

    public unsafe bool TryRead(Address address, void* buffer, nuint length)
    {
        return sys.process_read(Handle, address, (byte*)buffer, length) != 0;
    }

    public unsafe bool TryGetPath([NotNullWhen(true)] out string? path)
    {
        nuint length = 260;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.process_get_path(Handle, pBuf, &length) != 0)
            {
                path = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.process_get_path(Handle, pBuf, &length) != 0)
            {
                path = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
            else
            {
                path = null;
                ArrayPool<byte>.Shared.Return(rented);
                return false;
            }
        }
    }

    public unsafe Address GetModuleAddress(string moduleName)
    {
        fixed (byte* pModuleName = Encoding.UTF8.GetBytes(moduleName))
        {
            return sys.process_get_module_address(Handle, pModuleName, (nuint)moduleName.Length);
        }
    }

    public unsafe ulong GetModuleSize(string moduleName)
    {
        fixed (byte* pModuleName = Encoding.UTF8.GetBytes(moduleName))
        {
            return sys.process_get_module_size(Handle, pModuleName, (nuint)moduleName.Length);
        }
    }

    public unsafe bool TryGetModulePath(string moduleName, [NotNullWhen(true)] out string? path)
    {
        fixed (byte* pModuleName = Encoding.UTF8.GetBytes(moduleName))
        {
            nuint length = 260;

            Span<byte> buf = stackalloc byte[(int)length];
            fixed (byte* pBuf = buf)
            {
                if (sys.process_get_module_path(Handle, pModuleName, (nuint)moduleName.Length, pBuf, &length) != 0)
                {
                    path = Encoding.UTF8.GetString(buf[..(int)length]);
                    return true;
                }
            }

            byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
            fixed (byte* pBuf = rented)
            {
                if (sys.process_get_module_path(Handle, pModuleName, (nuint)moduleName.Length, pBuf, &length) != 0)
                {
                    path = Encoding.UTF8.GetString(rented[..(int)length]);
                    ArrayPool<byte>.Shared.Return(rented);
                    return true;
                }
                else
                {
                    path = null;
                    ArrayPool<byte>.Shared.Return(rented);
                    return false;
                }
            }
        }
    }

    public ulong MemoryRangeCount => sys.process_get_memory_range_count(Handle);

    public Address GetMemoryRangeAddress(ulong index)
    {
        return sys.process_get_memory_range_address(Handle, index);
    }

    public ulong GetMemoryRangeSize(ulong index)
    {
        return sys.process_get_memory_range_size(Handle, index);
    }

    public MemoryRangeFlags GetMemoryRangeFlags(ulong index)
    {
        return (MemoryRangeFlags)sys.process_get_memory_range_flags(Handle, index);
    }

    public static Process GetProcessById(ulong processId)
    {
        return new(sys.process_attach_by_pid(processId));
    }

    public static unsafe Process GetProcessByName(string processName)
    {
        fixed (byte* pProcessName = Encoding.UTF8.GetBytes(processName))
        {
            return new(sys.process_attach(pProcessName, (nuint)processName.Length));
        }
    }

    public static unsafe bool TryGetProcessesByName(string processName, [NotNullWhen(true)] out ulong[]? processIds)
    {
        fixed (byte* pProcessName = Encoding.UTF8.GetBytes(processName))
        {
            nuint length = 32;

            Span<ulong> buf = stackalloc ulong[(int)length];
            fixed (ulong* pBuf = buf)
            {
                if (sys.process_list_by_name(pProcessName, (nuint)processName.Length, pBuf, &length) != 0)
                {
                    processIds = buf[..(int)length].ToArray();
                    return true;
                }
            }

            ulong[] rented = ArrayPool<ulong>.Shared.Rent((int)length);
            fixed (ulong* pBuf = rented)
            {
                if (sys.process_list_by_name(pProcessName, (nuint)processName.Length, pBuf, &length) != 0)
                {
                    processIds = rented[..(int)length];
                    ArrayPool<ulong>.Shared.Return(rented);
                    return true;
                }
                else
                {
                    processIds = null;
                    ArrayPool<ulong>.Shared.Return(rented);
                    return false;
                }
            }
        }
    }

    public void Detach()
    {
        sys.process_detach(Handle);
    }
}
