using System;
using System.Buffers;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

public static class MemoryExtensions
{
    public static unsafe Address Deref(this Process process, Address address, params ReadOnlySpan<uint> offsets)
    {
        Address deref = address;

        foreach (uint offset in offsets)
        {
            if (!process.Owner.TryRead(deref, &deref, process.Is64Bit ? 0x8u : 0x4u))
            {
                return Address.Zero;
            }

            deref += offset;
        }

        return deref;
    }

    public static unsafe T Read<T>(this Process process, Address address, params ReadOnlySpan<uint> offsets)
        where T : unmanaged
    {
        Address deref = process.Deref(address, offsets);

        T value;
        if (process.Owner.TryRead(deref, &value, MemoryHelpers.GetNativeSizeOf<T>(process)))
        {
            return value;
        }

        return default;
    }

    public static unsafe void ReadArray<T>(this Process process, Span<T> buffer, Address address, params ReadOnlySpan<uint> offsets)
        where T : unmanaged
    {
        if (!process.Is64Bit && MemoryHelpers.IsNativeType<T>())
        {
            uint[]? rented = null;
            Span<uint> buf32 = buffer.Length <= 256
                ? stackalloc uint[256]
                : (rented = ArrayPool<uint>.Shared.Rent(buffer.Length));

            process.ReadArray(buf32[..buffer.Length], address, offsets);
            TensorPrimitives.ConvertTruncating<uint, ulong>(buf32[..buffer.Length], MemoryMarshal.Cast<T, ulong>(buffer));

            if (rented is not null)
            {
                ArrayPool<uint>.Shared.Return(rented);
            }

            return;
        }

        Address deref = process.Deref(address, offsets);

        fixed (T* pBuffer = buffer)
        {
            process.Owner.TryRead(deref, pBuffer, MemoryHelpers.GetNativeSizeOf<T>(process) * (uint)buffer.Length);
        }
    }

    public static unsafe string ReadString(
        this Process process,
        int maxLength,
        StringType stringType,
        Address address,
        params ReadOnlySpan<uint> offsets)
    {
        if (maxLength <= 0)
        {
            return string.Empty;
        }

        return stringType switch
        {
            StringType.Utf8 => process.ReadUtf8String(maxLength, address, offsets),
            StringType.Utf16 => process.ReadUtf16String(maxLength, address, offsets),
            _ => process.ReadAutoString(maxLength, address, offsets)
        };
    }

    private static unsafe string ReadUtf8String(this Process process, int maxLength, Address address, params ReadOnlySpan<uint> offsets)
    {
        byte[]? rented = null;
        Span<byte> buffer = maxLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(maxLength));

        process.ReadArray(buffer[..maxLength], address, offsets);
        string result = GetUtf8String(buffer[..maxLength]);

        if (rented is not null)
        {
            ArrayPool<byte>.Shared.Return(rented);
        }

        return result;
    }

    private static unsafe string ReadUtf16String(this Process process, int maxLength, Address address, params ReadOnlySpan<uint> offsets)
    {
        char[]? rented = null;
        Span<char> buffer = maxLength <= 512
            ? stackalloc char[512]
            : (rented = ArrayPool<char>.Shared.Rent(maxLength));

        process.ReadArray(buffer[..maxLength], address, offsets);
        string result = GetUtf16String(buffer[..maxLength]);

        if (rented is not null)
        {
            ArrayPool<char>.Shared.Return(rented);
        }

        return result;
    }

    private static unsafe string ReadAutoString(this Process process, int maxLength, Address address, params ReadOnlySpan<uint> offsets)
    {
        // Assume unicode for the worst-case scenario and just allocate maxLength * 2.
        byte[]? rented = null;
        Span<byte> buffer = maxLength * 2 <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(maxLength * 2));

        process.ReadArray(buffer[..(maxLength * 2)], address, offsets);

        string result;
        if (maxLength >= 2 && buffer is [> 0, 0, > 0, 0, ..])
        {
            Span<char> chars = MemoryMarshal.Cast<byte, char>(buffer[..(maxLength * 2)]);
            result = GetUtf16String(chars);
        }
        else
        {
            result = GetUtf8String(buffer[..maxLength]);
        }

        if (rented is not null)
        {
            ArrayPool<byte>.Shared.Return(rented);
        }

        return result;
    }

    private static string GetUtf8String(ReadOnlySpan<byte> buffer)
    {
        int length = buffer.IndexOf((byte)'\0');
        return length == -1
            ? Encoding.UTF8.GetString(buffer)
            : Encoding.UTF8.GetString(buffer[..length]);
    }

    private static string GetUtf16String(ReadOnlySpan<char> buffer)
    {
        int length = buffer.IndexOf('\0');
        return length == -1
            ? new(buffer)
            : new(buffer[..length]);
    }
}
