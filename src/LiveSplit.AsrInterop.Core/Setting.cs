using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents a setting's value.
/// </summary>
public readonly struct Setting
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified handle.
    /// </summary>
    /// <param name="handle">
    ///     The handle of the setting.
    /// </param>
    public Setting(ulong handle)
    {
        Handle = handle;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_map(ulong)"/>.
    /// </remarks>
    public Setting(SettingsMap value)
    {
        Handle = sys.setting_value_new_map(value.Handle);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_list(ulong)"/>.
    /// </remarks>
    public Setting(SettingsList value)
    {
        Handle = sys.setting_value_new_list(value.Handle);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_bool(byte)"/>.
    /// </remarks>
    public Setting(bool value)
    {
        Handle = sys.setting_value_new_bool((byte)(value ? 1 : 0));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_i64(long)"/>.
    /// </remarks>
    public Setting(long value)
    {
        Handle = sys.setting_value_new_i64(value);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_f64(double)"/>.
    /// </remarks>
    public Setting(double value)
    {
        Handle = sys.setting_value_new_f64(value);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Setting"/> struct
    ///     with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the new setting.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_new_string(byte*, nuint)"/>.
    /// </remarks>
    public unsafe Setting(string value)
    {
        fixed (byte* pValue = Encoding.UTF8.GetBytes(value))
        {
            Handle = sys.setting_value_new_string(pValue, (nuint)value.Length);
        }
    }

    /// <summary>
    ///     Gets the handle of the setting.
    /// </summary>
    public ulong Handle { get; }

    /// <summary>
    ///     Gets a value indicating whether the setting is valid.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the setting's handle is not zero;
    ///     otherwise, <see langword="false"/>.
    public bool IsValid => Handle != 0;

    /// <summary>
    ///     Gets the type of the setting's value.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_type(ulong)"/>.
    /// </remarks>
    public SettingValueType Type => (SettingValueType)sys.setting_value_get_type(Handle);

    /// <summary>
    ///     Tries to get the value of the setting as a map.
    /// </summary>
    /// <param name="value">
    ///     When this setting is a map, contains the value;
    ///     otherwise, an invalid map.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_map(ulong, ulong*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(out SettingsMap value)
    {
        ulong handle;
        if (sys.setting_value_get_map(Handle, &handle) == 0)
        {
            value = default;
            return false;
        }

        value = new(handle);
        return true;
    }

    /// <summary>
    ///     Tries to get the value of the setting as a list.
    /// </summary>
    /// <param name="value">
    ///     When this setting is a list, contains the value;
    ///     otherwise, an invalid list.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_list(ulong, ulong*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(out SettingsList value)
    {
        ulong handle;
        if (sys.setting_value_get_list(Handle, &handle) == 0)
        {
            value = default;
            return false;
        }

        value = new(handle);
        return true;
    }

    /// <summary>
    ///     Tries to get the value of the setting as a boolean.
    /// </summary>
    /// <param name="value">
    ///     When this setting is a boolean, contains the value;
    ///     otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_bool(ulong, byte*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(out bool value)
    {
        byte bValue;
        if (sys.setting_value_get_bool(Handle, &bValue) == 0)
        {
            value = default;
            return false;
        }

        value = bValue != 0;
        return true;
    }

    /// <summary>
    ///     Tries to get the value of the setting as an integer.
    /// </summary>
    /// <param name="value">
    ///     When this setting is an integer, contains the value;
    ///     otherwise, <c>0</c>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_i64(ulong, long*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(out long value)
    {
        fixed (long* pValue = &value)
        {
            return sys.setting_value_get_i64(Handle, pValue) != 0;
        }
    }

    /// <summary>
    ///     Tries to get the value of the setting as a floating-point number.
    /// </summary>
    /// <param name="value">
    ///     When this setting is a floating-point number, contains the value;
    ///     otherwise, <c>0.0</c>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_f64(ulong, double*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(out double value)
    {
        fixed (double* pValue = &value)
        {
            return sys.setting_value_get_f64(Handle, pValue) != 0;
        }
    }

    /// <summary>
    ///     Tries to get the value of the setting as a string.
    /// </summary>
    /// <param name="value">
    ///     When this setting is a string, contains the value;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_get_string(ulong, byte*, nuint*)"/>.
    /// </remarks>
    public unsafe bool TryGetValue([NotNullWhen(true)] out string? value)
    {
        nuint length = 256;

        Span<byte> buffer = stackalloc byte[(int)length];
        fixed (byte* pBuffer = buffer)
        {
            if (sys.setting_value_get_string(Handle, pBuffer, &length) != 0)
            {
                value = Encoding.UTF8.GetString(buffer[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuffer = rented)
        {
            if (sys.setting_value_get_string(Handle, pBuffer, &length) != 0)
            {
                value = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
        }

        value = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }

    /// <summary>
    ///     Frees the setting.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.setting_value_free(ulong)"/>.
    /// </remarks>
    public void Free()
    {
        sys.setting_value_free(Handle);
    }
}
