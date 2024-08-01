using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents a map of settings.
/// </summary>
public readonly struct SettingsMap
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SettingsMap"/> struct
    ///     with the specified handle.
    /// </summary>
    /// <param name="handle">
    ///     The handle of the settings map.
    /// </param>
    public SettingsMap(ulong handle)
    {
        Handle = handle;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SettingsMap"/> struct.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_new()"/>.
    /// </remarks>
    public SettingsMap()
    {
        Handle = sys.settings_map_new();
    }

    /// <summary>
    ///     Loads the current global settings map.
    /// </summary>
    /// <returns>
    ///     The current global settings map.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_load()"/>.
    /// </remarks>
    public static SettingsMap Load()
    {
        return new(sys.settings_map_load());
    }

    /// <summary>
    ///     Gets the handle of the settings map.
    /// </summary>
    public ulong Handle { get; }

    /// <summary>
    ///     Gets a value indicating whether the setting is valid.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the setting's handle is not zero;
    ///     otherwise, <see langword="false"/>.
    /// </value>
    public bool IsValid => Handle != 0;

    /// <summary>
    ///     Gets the number of settings in the map.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_len(ulong)"/>.
    /// </remarks>
    public ulong Count => sys.settings_map_len(Handle);

    /// <summary>
    ///     Gets the setting at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the setting to get.
    /// </param>
    /// <returns>
    ///     The setting at the specified index.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_get_value_by_index(ulong, ulong)"/>.
    /// </remarks>
    public Setting this[ulong index] => new(sys.settings_map_get_value_by_index(Handle, index));

    /// <summary>
    ///     Gets the setting with the specified key.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting to get or set.
    /// </param>
    /// <returns>
    ///     The setting with the specified key.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     The setting with the specified key was not present in the settings map.
    /// </exception>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_get(ulong, byte*, nuint)"/>.
    /// </remarks>
    public unsafe Setting this[string key]
    {
        get
        {
            if (!TryGetValue(key, out Setting value))
            {
                throw new KeyNotFoundException(
                    $"The setting '{key}' was not present in the settings map.");
            }

            return value;
        }
    }

    /// <summary>
    ///     Tries to get the value of the setting with the specified key.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting to get.
    /// </param>
    /// <param name="value">
    ///     If the key is found, contains the setting with the specified key;
    ///     otherwise, contains an invalid setting.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the key is found;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_get(ulong, byte*, nuint)"/>.
    /// </remarks>
    public unsafe bool TryGetValue(string key, out Setting value)
    {
        fixed (byte* pKey = Encoding.UTF8.GetBytes(key))
        {
            value = new(sys.settings_map_get(Handle, pKey, (nuint)key.Length));
            return value.IsValid;
        }
    }

    /// <summary>
    ///     Tries to get the key of the setting at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the setting.
    /// </param>
    /// <param name="key">
    ///     If the index is valid, contains the key of the setting at the specified index;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the index is valid;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_get_key_by_index(ulong, ulong, byte*, nuint*)"/>.
    /// </remarks>
    public unsafe bool TryGetKeyByIndex(ulong index, [NotNullWhen(true)] out string? key)
    {
        nuint length = 128;

        Span<byte> buffer = stackalloc byte[(int)length];
        fixed (byte* pBuffer = buffer)
        {
            if (sys.settings_map_get_key_by_index(Handle, index, pBuffer, &length) != 0)
            {
                key = Encoding.UTF8.GetString(buffer[..(int)length]);
                return true;
            }
        }

        byte[] rented = ArrayPool<byte>.Shared.Rent((int)length);
        fixed (byte* pBuffer = rented)
        {
            if (sys.settings_map_get_key_by_index(Handle, index, pBuffer, &length) != 0)
            {
                key = Encoding.UTF8.GetString(rented[..(int)length]);
                ArrayPool<byte>.Shared.Return(rented);
                return true;
            }
            else
            {
                key = null;
                ArrayPool<byte>.Shared.Return(rented);
                return false;
            }
        }
    }

    /// <summary>
    ///     Inserts the specified setting into the map.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting to insert.
    /// </param>
    /// <param name="value">
    ///     The setting to insert.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_insert(ulong, byte*, nuint, ulong)"/>.
    /// </remarks>
    public unsafe void Insert(string key, Setting value)
    {
        fixed (byte* pKey = Encoding.UTF8.GetBytes(key))
        {
            sys.settings_map_insert(Handle, pKey, (nuint)key.Length, value.Handle);
        }
    }

    /// <summary>
    ///     Stores the settings map as the current global settings map.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_store(ulong)"/>.
    /// </remarks>
    public void Store()
    {
        sys.settings_map_store(Handle);
    }

    /// <summary>
    ///     Stores the settings map as the current global settings map if it has not changed.
    /// </summary>
    /// <param name="old">
    ///     The settings map to compare against.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_store_if_unchanged(ulong, ulong)"/>.
    /// </remarks>
    public void StoreIfUnchanged(SettingsMap old)
    {
        sys.settings_map_store_if_unchanged(old.Handle, Handle);
    }

    /// <summary>
    ///     Copies the settings map.
    /// </summary>
    /// <returns>
    ///     A copy of the settings map.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_copy(ulong)"/>.
    /// </remarks>
    public SettingsMap Copy()
    {
        return new(sys.settings_map_copy(Handle));
    }

    /// <summary>
    ///     Frees the settings map.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_map_free(ulong)"/>.
    /// </remarks>
    public void Free()
    {
        sys.settings_map_free(Handle);
    }
}
