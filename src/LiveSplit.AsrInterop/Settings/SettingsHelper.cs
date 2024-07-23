using System;
using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public static partial class SettingsHelper
{
    public static Heading H1(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H1, children);
    }

    public static Heading H2(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H2, children);
    }

    public static Heading H3(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H3, children);
    }

    public static Heading H4(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H4, children);
    }

    public static Heading H5(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H5, children);
    }

    public static Heading H6(string title, params IEnumerable<Setting> children)
    {
        return new(title, HeadingLevel.H6, children);
    }

    public static Toggle Toggle(string key, bool defaultValue = false)
    {
        return new(key, key, defaultValue);
    }

    public static Toggle Toggle(string key, string description, bool defaultValue = false)
    {
        return new(key, description, defaultValue);
    }

    public static Choice<TEnum> Choice<TEnum>(string key, TEnum defaultValue = default)
        where TEnum : unmanaged, Enum
    {
        return new(key, key, defaultValue);
    }

    public static Choice<TEnum> Choice<TEnum>(string key, string description, TEnum defaultValue = default)
        where TEnum : unmanaged, Enum
    {
        return new(key, description, defaultValue);
    }

    public static FileSelect FileSelect(string key)
    {
        return new(key, key);
    }

    public static FileSelect FileSelect(string key, string description)
    {
        return new(key, description);
    }

    public static FileSelect WithMimeFilter(this FileSelect fileSelect, string pattern)
    {
        if (fileSelect.MimeFilters is { } existing)
        {
            return fileSelect with { MimeFilters = existing.Append(pattern) };
        }
        else
        {
            return fileSelect with { MimeFilters = [pattern] };
        }
    }

    public static FileSelect WithNameFilter(this FileSelect fileSelect, string pattern)
    {
        if (fileSelect.NameFilters is { } existing)
        {
            return fileSelect with { NameFilters = existing.Append((null, pattern)) };
        }
        else
        {
            return fileSelect with { NameFilters = [(null, pattern)] };
        }
    }

    public static FileSelect WithNameFilter(this FileSelect fileSelect, string description, string pattern)
    {
        if (fileSelect.NameFilters is { } existing)
        {
            return fileSelect with { NameFilters = existing.Append((description, pattern)) };
        }
        else
        {
            return fileSelect with { NameFilters = [(description, pattern)] };
        }
    }

    public static TSetting Tooltip<TSetting>(this TSetting setting, string tooltip)
        where TSetting : Setting
    {
        return setting with { Tooltip = tooltip };
    }

    public static void Register(this IEnumerable<Setting> settings)
    {
        foreach (Setting s in settings)
        {
            s.Register();

            if (s.Tooltip is not null)
            {
                UserSettings.SetTooltip(s.Key, s.Tooltip);
            }
        }
    }
}
