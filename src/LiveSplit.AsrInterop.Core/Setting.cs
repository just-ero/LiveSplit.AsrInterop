using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

public readonly struct Setting
{
    public Setting(ulong handle)
    {
        Handle = handle;
    }

    public Setting(SettingsMap value)
    {
        Handle = sys.setting_value_new_map(value.Handle);
    }

    public Setting(SettingsList value)
    {
        Handle = sys.setting_value_new_list(value.Handle);
    }

    public Setting(bool value)
    {
        Handle = sys.setting_value_new_bool((byte)(value ? 1 : 0));
    }

    public Setting(long value)
    {
        Handle = sys.setting_value_new_i64(value);
    }

    public Setting(double value)
    {
        Handle = sys.setting_value_new_f64(value);
    }

    public unsafe Setting(string value)
    {
        fixed (byte* pValue = Encoding.UTF8.GetBytes(value))
        {
            Handle = sys.setting_value_new_string(pValue, (nuint)value.Length);
        }
    }

    public ulong Handle { get; }
    public bool IsValid => Handle != 0;

    public SettingValueType Type => (SettingValueType)sys.setting_value_get_type(Handle);

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

    public unsafe bool TryGetValue(out long value)
    {
        fixed (long* pValue = &value)
        {
            return sys.setting_value_get_i64(Handle, pValue) != 0;
        }
    }

    public unsafe bool TryGetValue(out double value)
    {
        fixed (double* pValue = &value)
        {
            return sys.setting_value_get_f64(Handle, pValue) != 0;
        }
    }

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

    public void Free()
    {
        sys.setting_value_free(Handle);
    }
}
