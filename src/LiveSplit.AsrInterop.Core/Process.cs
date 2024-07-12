using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Provides access to marshalled <see href="https://github.com/LiveSplit/asr">asr</see> process functions.
/// </summary>
public static unsafe class Process
{
    private const int MaxPath = 260;

    /// <summary>
    ///     Attaches to a process with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the process to attach to.
    /// </param>
    /// <returns>
    ///     A handle to the youngest process with the specified <paramref name="name"/> when the method succeeds;
    ///     otherwise, <see cref="ProcessHandle.Zero"/>.
    /// </returns>
    public static ProcessHandle Attach(string name)
    {
        return Attach(Encoding.UTF8.GetBytes(name));
    }

    /// <summary>
    ///     Attaches to a process with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the process to attach to.
    /// </param>
    /// <returns>
    ///     A handle to the youngest process with the specified <paramref name="name"/> when the method succeeds;
    ///     otherwise, <see cref="ProcessHandle.Zero"/>.
    /// </returns>
    public static ProcessHandle Attach(ReadOnlySpan<byte> name)
    {
        fixed (byte* pName = name)
        {
            return sys.process_attach(pName, (nuint)name.Length);
        }
    }

    /// <summary>
    ///     Attaches to a process with the specified process ID.
    /// </summary>
    /// <param name="id">
    ///     The process ID of the process to attach to.
    /// </param>
    /// <returns>
    ///     A handle to the process with the specified process ID when the method succeeds;
    ///     otherwise, <see cref="ProcessHandle.Zero"/>.
    /// </returns>
    public static ProcessHandle AttachByPid(ulong id)
    {
        return sys.process_attach_by_pid(id);
    }

    /// <summary>
    ///     Lists all processes matching the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the processes to list.
    /// </param>
    /// <param name="ids">
    ///     An array of process IDs of the processes with the specified <paramref name="name"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ListByName(string name, [NotNullWhen(true)] out ulong[]? ids)
    {
        return ListByName(Encoding.UTF8.GetBytes(name), out ids);
    }

    /// <summary>
    ///     Lists all processes matching the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the processes to list.
    /// </param>
    /// <param name="ids">
    ///     An array of process IDs of the processes with the specified <paramref name="name"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ListByName(ReadOnlySpan<byte> name, [NotNullWhen(true)] out ulong[]? ids)
    {
        fixed (byte* pName = name)
        {
            nuint length = 32;

            Span<ulong> buf = stackalloc ulong[(int)length];
            fixed (ulong* pBuf = buf)
            {
                if (sys.process_list_by_name(pName, (nuint)name.Length, pBuf, &length) != 0)
                {
                    ids = buf[..(int)length].ToArray();
                    return true;
                }
            }

            ulong[] rented = ArrayPool<ulong>.Shared.Rent((int)length);
            fixed (ulong* pBuf = rented)
            {
                if (sys.process_list_by_name(pName, (nuint)name.Length, pBuf, &length) != 0)
                {
                    ids = rented[..(int)length];
                    ArrayPool<ulong>.Shared.Return(rented);
                    return true;
                }
            }

            ids = null;
            ArrayPool<ulong>.Shared.Return(rented);
            return false;
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the specified process is running.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the process is running;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsOpen(ProcessHandle processHandle)
    {
        return sys.process_is_open(processHandle) != 0;
    }

    /// <summary>
    ///     Detaches from the specified process and closes the handle.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    public static void Detach(ProcessHandle processHandle)
    {
        sys.process_detach(processHandle);
    }

    /// <summary>
    ///     Gets the full WASI file system path of the specified process' executable.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="path">
    ///     The fully qualified path when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns></returns>
    /// <remarks>
    ///     A file path in the form of <c>C:\path\to\executable.exe</c> will be returned as <c>/mnt/c/path/to/executable.exe</c>.
    /// </remarks>
    public static bool GetPath(ProcessHandle processHandle, [NotNullWhen(true)] out string? path)
    {
        nuint length = MaxPath;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.process_get_path(processHandle, pBuf, &length) != 0)
            {
                path = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.process_get_path(processHandle, pBuf, &length) != 0)
            {
                path = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
        }

        path = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }

    /// <summary>
    ///     Reads a value of the specified size at the specified address in the target process.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="address">
    ///     The address at which to begin reading.
    /// </param>
    /// <param name="buffer">
    ///     A pointer to the buffer that receives the data.
    /// </param>
    /// <param name="length">
    ///     The size of the buffer, in bytes.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Read(ProcessHandle processHandle, UAddress address, void* buffer, nuint length)
    {
        return sys.process_read(processHandle, address, (byte*)buffer, length) != 0;
    }

    /// <summary>
    ///     Gets the memory address where the module with the specified <paramref name="name"/> was loaded.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <returns>
    ///     The load address of the module when the method succeeds;
    ///     otherwise, <see cref="UAddress.Zero"/>.
    /// </returns>
    public static UAddress GetModuleAddress(ProcessHandle processHandle, string name)
    {
        return GetModuleAddress(processHandle, Encoding.UTF8.GetBytes(name));
    }

    /// <summary>
    ///     Gets the memory address where the module with the specified <paramref name="name"/> was loaded.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <returns>
    ///     The load address of the module when the method succeeds;
    ///     otherwise, <see cref="UAddress.Zero"/>.
    /// </returns>
    public static UAddress GetModuleAddress(ProcessHandle processHandle, ReadOnlySpan<byte> name)
    {
        fixed (byte* pName = name)
        {
            return sys.process_get_module_address(processHandle, pName, (nuint)name.Length);
        }
    }

    /// <summary>
    ///     Gets the amount of memory that is required to load the module with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <returns>
    ///     The size, in bytes, of the memory that the module occupies when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </returns>
    public static ulong GetModuleSize(ProcessHandle processHandle, string name)
    {
        return GetModuleSize(processHandle, Encoding.UTF8.GetBytes(name));
    }

    /// <summary>
    ///     Gets the amount of memory that is required to load the module with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <returns>
    ///     The size, in bytes, of the memory that the module occupies when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </returns>
    public static ulong GetModuleSize(ProcessHandle processHandle, ReadOnlySpan<byte> name)
    {
        fixed (byte* pName = name)
        {
            return sys.process_get_module_size(processHandle, pName, (nuint)name.Length);
        }
    }

    /// <summary>
    ///     Gets the full WASI file system path of the module with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <param name="path">
    ///     The fully qualified path that defines the location of the module when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool GetModulePath(ProcessHandle processHandle, string name, [NotNullWhen(true)] out string? path)
    {
        return GetModulePath(processHandle, Encoding.UTF8.GetBytes(name), out path);
    }

    /// <summary>
    ///     Gets the full WASI file system path of the module with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="name">
    ///     The name of the module.
    /// </param>
    /// <param name="path">
    ///     The fully qualified path that defines the location of the module when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool GetModulePath(ProcessHandle processHandle, ReadOnlySpan<byte> name, [NotNullWhen(true)] out string? path)
    {
        fixed (byte* pName = name)
        {
            nuint length = MaxPath;

            Span<byte> buf = stackalloc byte[(int)length];
            fixed (byte* pBuf = buf)
            {
                if (sys.process_get_module_path(processHandle, pName, (nuint)name.Length, pBuf, &length) != 0)
                {
                    path = Encoding.UTF8.GetString(buf[..(int)length]);
                    return true;
                }
            }

            byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
            fixed (byte* pBuf = rented)
            {
                if (sys.process_get_module_path(processHandle, pName, (nuint)name.Length, pBuf, &length) != 0)
                {
                    path = Encoding.UTF8.GetString(rented[..(int)length]);
                    ArrayPool<byte>.Shared.Return(rented);
                    return true;
                }
            }

            path = null;
            ArrayPool<byte>.Shared.Return(rented);
            return false;
        }
    }

    /// <summary>
    ///     Gets the amount of ranges of pages in the target process' virtual address space.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <returns>
    ///     The number of memory ranges when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </returns>
    public static ulong GetMemoryRangeCount(ProcessHandle processHandle)
    {
        return sys.process_get_memory_range_count(processHandle);
    }

    /// <summary>
    ///     Gets the address of the specified memory range.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="index">
    ///     The index of the memory range.
    /// </param>
    /// <returns>
    ///     The address of the memory range when the method succeeds;
    /// </returns>
    public static UAddress GetMemoryRangeAddress(ProcessHandle processHandle, ulong index)
    {
        return sys.process_get_memory_range_address(processHandle, index);
    }

    /// <summary>
    ///     Gets the size of the specified memory range.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="index">
    ///     The index of the memory range.
    /// </param>
    /// <returns>
    ///     The size of the memory range when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </returns>
    public static ulong GetMemoryRangeSize(ProcessHandle processHandle, ulong index)
    {
        return sys.process_get_memory_range_size(processHandle, index);
    }

    /// <summary>
    ///     Gets the flags of the specified memory range.
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the target process.
    /// </param>
    /// <param name="index">
    ///     The index of the memory range.
    /// </param>
    /// <returns>
    ///     The flags of the memory range when the method succeeds;
    ///     otherwise, <see cref="MemoryRangeFlags.None"/>.
    /// </returns>
    public static MemoryRangeFlags GetMemoryRangeFlags(ProcessHandle processHandle, ulong index)
    {
        return (MemoryRangeFlags)sys.process_get_memory_range_flags(processHandle, index);
    }
}
