using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Provides access to marshalled <see href="https://github.com/LiveSplit/asr">asr</see> settings map functions.
/// </summary>
public static unsafe class SettingsMap
{
    /// <summary>
    ///     Creates a new map of settings.
    /// </summary>
    /// <remarks>
    ///     The caller is responsible for freeing the returned map using <see cref="Free"/>.
    /// </remarks>
    public static SettingsMapHandle New()
    {
        return sys.settings_map_new();
    }

    /// <summary>
    ///     Frees the provided <paramref name="map"/>.
    /// </summary>
    public static void Free(SettingsMapHandle map)
    {
        sys.settings_map_free(map);
    }

    /// <summary>
    ///     Loads a copy of the current global settings map.
    /// </summary>
    /// <remarks>
    ///     The caller is responsible for freeing the returned map using <see cref="Free"/>.
    /// </remarks>
    public static SettingsMapHandle Load()
    {
        return sys.settings_map_load();
    }

    /// <summary>
    ///     Stores a copy of the provided <paramref name="map"/> as the new global settings map, replacing the current one.
    /// </summary>
    /// <remarks>
    ///     The caller is still responsible for freeing the map using <see cref="Free"/>.
    /// </remarks>
    public static void Store(SettingsMapHandle map)
    {
        sys.settings_map_store(map);
    }

    /// <summary>
    ///     Stores the specified <paramref name="newMap"/> as the new global settings map
    ///     if it is different from the specified <paramref name="oldMap"/>.
    /// </summary>
    /// <param name="oldMap">
    ///     The old map to compare against.
    /// </param>
    /// <param name="newMap">
    ///     The new map to store if it is different from the old map.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the new map was stored;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool StoreIfUnchanged(SettingsMapHandle oldMap, SettingsMapHandle newMap)
    {
        return sys.settings_map_store_if_unchanged(oldMap, newMap) != 0;
    }

    /// <summary>
    ///     Creates a copy of the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map to copy.
    /// </param>
    /// <returns>
    ///     A copy of the map.
    /// </returns>
    /// <remarks>
    ///     Changes to the returned map do not affect the original map.<br/>
    ///     The caller is responsible for freeing the returned map using <see cref="Free"/>.
    /// </remarks>
    public static SettingsMapHandle Copy(SettingsMapHandle map)
    {
        return sys.settings_map_copy(map);
    }

    /// <summary>
    ///     Inserts a new key-value pair into the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map to insert the key-value pair into.
    /// </param>
    /// <param name="key">
    ///     The key to insert.
    /// </param>
    /// <param name="value">
    ///     The value to insert.
    /// </param>
    public static void Insert(SettingsMapHandle map, string key, SettingValueHandle value)
    {
        Insert(map, Encoding.UTF8.GetBytes(key), value);
    }

    /// <summary>
    ///     Inserts a new key-value pair into the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map to insert the key-value pair into.
    /// </param>
    /// <param name="key">
    ///     The key to insert.
    /// </param>
    /// <param name="value">
    ///     The value corresponding to the key.
    /// </param>
    public static void Insert(SettingsMapHandle map, ReadOnlySpan<byte> key, SettingValueHandle value)
    {
        fixed (byte* pKey = key)
        {
            sys.settings_map_insert(map, pKey, (nuint)key.Length, value);
        }
    }

    /// <summary>
    ///     Gets the value associated with the specified <paramref name="key"/> in the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map to get the value from.
    /// </param>
    /// <param name="key">
    ///     The key whose corresponding value to get.
    /// </param>
    /// <returns>
    ///     The value associated with the specified <paramref name="key"/> if it exists;
    ///     otherwise, <see cref="SettingValueHandle.Zero"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="SettingValue.Free"/>.
    /// </remarks>
    public static SettingValueHandle Get(SettingsMapHandle map, string key)
    {
        return Get(map, Encoding.UTF8.GetBytes(key));
    }

    /// <summary>
    ///     Gets the value associated with the specified <paramref name="key"/> in the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map to get the value from.
    /// </param>
    /// <param name="key">
    ///     The key whose corresponding value to get.
    /// </param>
    /// <returns>
    ///     The value associated with the specified <paramref name="key"/> if it exists;
    ///     otherwise, <see cref="SettingValueHandle.Zero"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="SettingValue.Free"/>.
    /// </remarks>
    public static SettingValueHandle Get(SettingsMapHandle map, ReadOnlySpan<byte> key)
    {
        fixed (byte* pKey = key)
        {
            return sys.settings_map_get(map, pKey, (nuint)key.Length);
        }
    }

    /// <summary>
    ///     Gets the length of the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map of which to get the length.
    /// </param>
    /// <returns>
    ///     The length of the map.
    /// </returns>
    public static ulong Len(SettingsMapHandle map)
    {
        return sys.settings_map_len(map);
    }

    /// <summary>
    ///     Gets the key at the specified <paramref name="index"/> in the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map which contains the key.
    /// </param>
    /// <param name="index">
    ///     The index of the key.
    /// </param>
    /// <param name="key">
    ///     The key at the specified index when the method succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the key was successfully retrieved;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool GetKeyByIndex(SettingsMapHandle map, ulong index, [NotNullWhen(true)] out string? key)
    {
        nuint length = 128;

        Span<byte> buf = stackalloc byte[(int)length];
        fixed (byte* pBuf = buf)
        {
            if (sys.settings_map_get_key_by_index(map, index, pBuf, &length) != 0)
            {
                key = Encoding.UTF8.GetString(buf[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuf = rented)
        {
            if (sys.settings_map_get_key_by_index(map, index, pBuf, &length) != 0)
            {
                key = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
        }

        key = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }

    /// <summary>
    ///     Gets the value at the specified <paramref name="index"/> in the provided <paramref name="map"/>.
    /// </summary>
    /// <param name="map">
    ///     The map which contains the value.
    /// </param>
    /// <param name="index">
    ///     The index of the value.
    /// </param>
    /// <returns>
    ///     The value at the specified index when the method succeeds;
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="SettingValue.Free"/>.
    /// </remarks>
    public static SettingValueHandle GetValueByIndex(SettingsMapHandle map, ulong index)
    {
        return sys.settings_map_get_value_by_index(map, index);
    }
}
