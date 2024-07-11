using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

public static unsafe class SettingValue
{
    /// <summary>
    ///     Creates a new map setting value.
    /// </summary>
    /// <param name="map">
    ///     The settings map to create the setting value from.
    /// </param>
    /// <returns>
    ///     A copy of the provided <paramref name="map"/> as a setting value.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.<br/>
    ///     The caller retains ownership of the provided <paramref name="map"/>.
    /// </remarks>
    public static USettingValue New(USettingsMap map)
    {
        return sys.setting_value_new_map(map);
    }

    /// <summary>
    ///     Creates a new list setting value.
    /// </summary>
    /// <param name="list">
    ///     The settings list to create the setting value from.
    /// </param>
    /// <returns>
    ///     A copy of the provided <paramref name="list"/> as a setting value.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.<br/>
    ///     The caller retains ownership of the provided <paramref name="list"/>.
    /// </remarks>
    public static USettingValue New(USettingsList list)
    {
        return sys.setting_value_new_list(list);
    }

    /// <summary>
    ///     Creates a new boolean setting value.
    /// </summary>
    /// <param name="value">
    ///     The boolean value to create the setting value from.
    /// </param>
    /// <returns>
    ///     A setting value representing the provided <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.
    /// </remarks>
    public static USettingValue New(bool value)
    {
        return sys.setting_value_new_bool((byte)(value ? 1 : 0));
    }

    /// <summary>
    ///     Creates a new 64-bit signed integer setting value.
    /// </summary>
    /// <param name="value">
    ///     The integer value to create the setting value from.
    /// </param>
    /// <returns>
    ///     A setting value representing the provided <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.
    /// </remarks>
    public static USettingValue New(long value)
    {
        return sys.setting_value_new_i64(value);
    }

    /// <summary>
    ///     Creates a new 64-bit floating-point setting value.
    /// </summary>
    /// <param name="value">
    ///     The floating-point value to create the setting value from.
    /// </param>
    /// <returns>
    ///     A setting value representing the provided <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.
    /// </remarks>
    public static USettingValue New(double value)
    {
        return sys.setting_value_new_f64(value);
    }

    /// <summary>
    ///     Creates a new string setting value.
    /// </summary>
    /// <param name="value">
    ///     The string value to create the setting value from.
    /// </param>
    /// <returns>
    ///     A setting value representing the provided <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.
    /// </remarks>
    public static USettingValue New(string value)
    {
        return New(Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    ///     Creates a new string setting value.
    /// </summary>
    /// <param name="value">
    ///     The string value to create the setting value from.
    /// </param>
    /// <returns>
    ///     A setting value representing the provided <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="Free"/>.
    /// </remarks>
    public static USettingValue New(ReadOnlySpan<byte> value)
    {
        fixed (byte* pValue = value)
        {
            return sys.setting_value_new_string(pValue, (nuint)value.Length);
        }
    }

    /// <summary>
    ///     Frees the provided setting value.
    /// </summary>
    /// <param name="setting">
    ///     The setting value to free.
    /// </param>
    public static void Free(USettingValue setting)
    {
        sys.setting_value_free(setting);
    }

    /// <summary>
    ///     Gets the type of the provided setting value.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the type.s
    /// </param>
    /// <returns>
    ///     The type of the provided setting value.
    /// </returns>
    public static SettingValueType GetType(USettingValue setting)
    {
        return (SettingValueType)sys.setting_value_get_type(setting);
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a settings map.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the map.
    /// </param>
    /// <param name="map">
    ///     The map value of the provided setting value when the method succeeds;
    ///     otherwise, <see cref="USettingsMap.None"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="USettingsMap.Free"/>.
    ///     The caller retains ownership of the provided <paramref name="setting"/>.
    /// </remarks>
    public static bool Get(USettingValue setting, out USettingsMap map)
    {
        fixed (USettingsMap* pMap = &map)
        {
            return sys.setting_value_get_map(setting, (ulong*)pMap) != 0;
        }
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a settings list.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the list.
    /// </param>
    /// <param name="list">
    ///     The list value of the provided setting value when the method succeeds;
    ///     otherwise, <see cref="USettingsList.None"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="USettingsList.Free"/>.
    ///     The caller retains ownership of the provided <paramref name="setting"/>.
    /// </remarks>
    public static bool Get(USettingValue setting, out USettingsList list)
    {
        fixed (USettingsList* pList = &list)
        {
            return sys.setting_value_get_list(setting, (ulong*)pList) != 0;
        }
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a boolean.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the boolean.
    /// </param>
    /// <param name="value">
    ///     The boolean value of the provided setting value when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Get(USettingValue setting, out bool value)
    {
        fixed (bool* pValue = &value)
        {
            return sys.setting_value_get_bool(setting, (byte*)pValue) != 0;
        }
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a 64-bit signed integer.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the integer.
    /// </param>
    /// <param name="value">
    ///     The integer value of the provided setting value when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Get(USettingValue setting, out long value)
    {
        fixed (long* pValue = &value)
        {
            return sys.setting_value_get_i64(setting, pValue) != 0;
        }
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a 64-bit floating-point number.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the floating-point number.
    /// </param>
    /// <param name="value">
    ///     The floating-point number value of the provided setting value when the method succeeds;
    ///     otherwise, <c>0</c>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Get(USettingValue setting, out double value)
    {
        fixed (double* pValue = &value)
        {
            return sys.setting_value_get_f64(setting, pValue) != 0;
        }
    }

    /// <summary>
    ///     Gets the value of the provided setting value as a string.
    /// </summary>
    /// <param name="setting">
    ///     The setting value of which to get the string.
    /// </param>
    /// <param name="value">
    ///     The string value of the provided setting value when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Get(USettingValue setting, [NotNullWhen(true)] out string? value)
    {
        nuint length = 256;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.setting_value_get_string(setting, pBuf, &length) != 0)
            {
                value = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.setting_value_get_string(setting, pBuf, &length) != 0)
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
}
