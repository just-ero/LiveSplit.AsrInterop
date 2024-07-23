using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

public readonly struct SettingsMap
{
    public SettingsMap(ulong handle)
    {
        Handle = handle;
    }

    public SettingsMap()
    {
        Handle = sys.settings_map_new();
    }

    public static SettingsMap Load()
    {
        return new(sys.settings_map_load());
    }

    public ulong Handle { get; }
    public bool IsValid => Handle != 0;

    public ulong Count => sys.settings_map_len(Handle);

    public Setting this[ulong index] => new(sys.settings_map_get_value_by_index(Handle, index));

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
        set
        {
            fixed (byte* pKey = Encoding.UTF8.GetBytes(key))
            {
                sys.settings_map_insert(Handle, pKey, (nuint)key.Length, value.Handle);
            }
        }
    }

    public unsafe bool TryGetValue(string key, out Setting value)
    {
        fixed (byte* pKey = Encoding.UTF8.GetBytes(key))
        {
            value = new(sys.settings_map_get(Handle, pKey, (nuint)key.Length));
            return value.IsValid;
        }
    }

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
        }

        key = null;
        ArrayPool<byte>.Shared.Return(rented);
        return false;
    }

    public void Store()
    {
        sys.settings_map_store(Handle);
    }

    public void StoreIfUnchanged(SettingsMap old)
    {
        sys.settings_map_store_if_unchanged(old.Handle, Handle);
    }

    public SettingsMap Copy()
    {
        return new(sys.settings_map_copy(Handle));
    }

    public void Free()
    {
        sys.settings_map_free(Handle);
    }
}
