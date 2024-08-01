using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Extensions;

public static class MemoryExtensions
{
    public static unsafe T Read<T>(this Process process, Address address)
        where T : unmanaged
    {
        T value;
        if (process.TryRead(address, &value, (nuint)sizeof(T)))
        {
            return value;
        }

        return default;
    }

    public static unsafe void ReadArray<T>(this Process process, Span<T> buffer, Address address)
        where T : unmanaged
    {
        fixed (T* pBuffer = buffer)
        {
            process.TryRead(address, pBuffer, (nuint)(sizeof(T) * buffer.Length));
        }
    }

    public static unsafe string ReadString(this Process process, int maxLength, StringType stringType, Address address)
    {
        if (maxLength <= 0)
        {
            return string.Empty;
        }

        return stringType switch
        {
            StringType.Utf8 => process.ReadUtf8String(maxLength, address),
            StringType.Utf16 => process.ReadUtf16String(maxLength, address),
            _ => process.ReadAutoString(maxLength, address)
        };
    }

    private static unsafe string ReadUtf8String(this Process process, int maxLength, Address address)
    {
        byte[]? rented = null;
        Span<byte> buffer = maxLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(maxLength));

        process.ReadArray(buffer[..maxLength], address);
        string result = GetUtf8String(buffer[..maxLength]);

        if (rented is not null)
        {
            ArrayPool<byte>.Shared.Return(rented);
        }

        return result;
    }

    private static unsafe string ReadUtf16String(this Process process, int maxLength, Address address)
    {
        char[]? rented = null;
        Span<char> buffer = maxLength <= 512
            ? stackalloc char[512]
            : (rented = ArrayPool<char>.Shared.Rent(maxLength));

        process.ReadArray(buffer[..maxLength], address);
        string result = GetUtf16String(buffer[..maxLength]);

        if (rented is not null)
        {
            ArrayPool<char>.Shared.Return(rented);
        }

        return result;
    }

    private static unsafe string ReadAutoString(this Process process, int maxLength, Address address)
    {
        // Assume unicode for the worst-case scenario and just allocate maxLength * 2.
        byte[]? rented = null;
        Span<byte> buffer = maxLength * 2 <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(maxLength * 2));

        process.ReadArray(buffer[..(maxLength * 2)], address);

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
