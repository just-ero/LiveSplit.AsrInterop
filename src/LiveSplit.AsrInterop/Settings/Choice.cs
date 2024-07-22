using System;
using System.ComponentModel;
using System.Reflection;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public sealed record Choice<TEnum>(
    string Key,
    string Title,
    TEnum Default = default) : Setting(Key) where TEnum : unmanaged, Enum
{
    public override void Register()
    {
        UserSettings.AddChoice(Key, Title, $"{typeof(TEnum).Name}.{Default}");

        foreach (string name in Enum.GetNames<TEnum>())
        {
            string key = $"{typeof(TEnum).Name}.{name}";
            string? desc = typeof(TEnum).GetField(name)?.GetCustomAttribute<DescriptionAttribute>()?.Description;

            UserSettings.AddChoiceOption(Key, key, desc ?? key);
        }
    }
}
