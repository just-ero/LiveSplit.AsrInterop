using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Provides access to marshalled asr runtime functions.
/// </summary>
public static unsafe class Runtime
{
    /// <summary>
    ///     Sets the tick rate of the runtime.
    ///     Dictates the maximum number of times per second the runtime will execute the `update` function.
    /// </summary>
    /// <param name="ticksPerSecond">
    ///     The tick rate, in ticks per second.
    /// </param>
    public static void SetTickRate(double ticksPerSecond)
    {
        sys.runtime_set_tick_rate(ticksPerSecond);
    }

    /// <summary>
    ///     Prints the specified <paramref name="message"/> to attached trace viewers and debuggers.
    /// </summary>
    /// <param name="message">
    ///     The message to print.
    /// </param>
    public static void PrintMessage(string message)
    {
        PrintMessage(Encoding.UTF8.GetBytes(message));
    }

    /// <summary>
    ///     Prints the specified <paramref name="message"/> to attached trace viewers and debuggers.
    /// </summary>
    /// <param name="message">
    ///     The message to print.
    /// </param>
    public static void PrintMessage(ReadOnlySpan<byte> message)
    {
        fixed (byte* pMessage = message)
        {
            sys.runtime_print_message(pMessage, (nuint)message.Length);
        }
    }

    /// <summary>
    ///     Queries the name of the operating system the runtime is running on.
    /// </summary>
    /// <param name="os">
    ///     The name of the operating system the runtime is running on,
    ///     or <see langword="null"/> if the query failed.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetOs([NotNullWhen(true)] out string? os)
    {
        nuint length = 64;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.runtime_get_os(pBuf, &length) != 0)
            {
                os = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.runtime_get_os(pBuf, &length) != 0)
            {
                os = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
        }

        os = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }

    /// <summary>
    ///     Queries the architecture of the operating system the runtime is running on.
    /// </summary>
    /// <param name="arch">
    ///     The architecture of the operating system the runtime is running on,
    ///     or <see langword="null"/> if the query failed.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetArch([NotNullWhen(true)] out string? arch)
    {
        nuint length = 64;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.runtime_get_arch(pBuf, &length) != 0)
            {
                arch = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.runtime_get_arch(pBuf, &length) != 0)
            {
                arch = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
        }

        arch = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }
}
