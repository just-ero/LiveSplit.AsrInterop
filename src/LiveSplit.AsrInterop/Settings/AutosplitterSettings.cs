using System;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public class AutosplitterSettings
{
    public SettingsMap Map { get; set; }

    public bool GetToggle(string key)
    {
        if (!Map[key].TryGetValue(out bool value))
        {
            throw new ArgumentException(
                $"The setting '{key}' was not a toggle.");
        }

        return value;
    }

    public TEnum GetChoice<TEnum>(string key)
        where TEnum : unmanaged, Enum
    {
        if (!Map[key].TryGetValue(out string? name))
        {
            throw new ArgumentException(
                $"The setting '{key}' was not a combobox.");
        }

        if (!Enum.TryParse(name, out TEnum value))
        {
            throw new ArgumentException(
                $"The setting '{key}' could not be parsed as an enum of type '{typeof(TEnum)}'.");
        }

        return value;
    }
}
