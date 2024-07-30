using System;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop;

public interface IAutosplitterSettings
{
    void RegisterSettings();

    SettingsMap Map { get; set; }

    bool GetToggle(string key)
    {
        if (!Map[key].TryGetValue(out bool value))
        {
            throw new ArgumentException(
                $"The setting '{key}' was not a toggle.");
        }

        return value;
    }

    TEnum GetChoice<TEnum>(string key)
        where TEnum : unmanaged, Enum
    {
        if (!Map[key].TryGetValue(out string? name))
        {
            throw new ArgumentException(
                $"The setting '{key}' was not a choice.");
        }

        if (!Enum.TryParse(name, out TEnum value))
        {
            throw new ArgumentException(
                $"The setting '{key}' could not be parsed as an enum of type '{typeof(TEnum)}'.");
        }

        return value;
    }

    bool GetChoice(string key, string option)
    {
        if (!Map[key].TryGetValue(out string? value))
        {
            throw new ArgumentException(
                $"The setting '{key}' was not a choice.");
        }

        return value == option;
    }
}
