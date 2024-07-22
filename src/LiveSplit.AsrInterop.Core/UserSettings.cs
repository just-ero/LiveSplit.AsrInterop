using System;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

public static unsafe class UserSettings
{
    /// <summary>
    ///     Adds a title to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the title.
    /// </param>
    /// <param name="title">
    ///     The title to add.
    /// </param>
    /// <param name="level">
    ///     The heading level of the title.
    /// </param>
    public static void AddTitle(string key, string title, uint level)
    {
        AddTitle(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(title), level);
    }

    /// <summary>
    ///     Adds a title to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the title.
    /// </param>
    /// <param name="title">
    ///     The title to add.
    /// </param>
    /// <param name="level">
    ///     The heading level of the title.
    /// </param>
    public static void AddTitle(ReadOnlySpan<byte> key, ReadOnlySpan<byte> title, uint level)
    {
        fixed (byte* pKey = key, pTitle = title)
        {
            sys.user_settings_add_title(pKey, (nuint)key.Length, pTitle, (nuint)title.Length, level);
        }
    }

    /// <summary>
    ///     Adds a boolean setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    /// <param name="defaultValue">
    ///     The default value of the setting.
    /// </param>
    /// <returns>
    ///     The specified default value or the value the user has set.
    /// </returns>
    public static bool AddBool(string key, string description, bool defaultValue)
    {
        return AddBool(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(description), defaultValue);
    }

    /// <summary>
    ///     Adds a boolean setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    /// <param name="defaultValue">
    ///     The default value of the setting.
    /// </param>
    /// <returns>
    ///     The specified default value or the value the user has set.
    /// </returns>
    public static bool AddBool(ReadOnlySpan<byte> key, ReadOnlySpan<byte> description, bool defaultValue)
    {
        fixed (byte* pKey = key, pDescription = description)
        {
            return sys.user_settings_add_bool(
                pKey,
                (nuint)key.Length,
                pDescription,
                (nuint)description.Length,
                (byte)(defaultValue ? 1 : 0)) != 0;
        }
    }

    /// <summary>
    ///     Adds a choice setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    /// <param name="defaultOptionKey">
    ///     The default option key of the setting.
    /// </param>
    public static void AddChoice(string key, string description, string defaultOptionKey)
    {
        AddChoice(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(description), Encoding.UTF8.GetBytes(defaultOptionKey));
    }

    /// <summary>
    ///     Adds a choice setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    /// <param name="defaultOptionKey">
    ///     The default option key of the setting.
    /// </param>
    public static void AddChoice(ReadOnlySpan<byte> key, ReadOnlySpan<byte> description, ReadOnlySpan<byte> defaultOptionKey)
    {
        fixed (byte* pKey = key, pDescription = description, pDefaultOptionKey = defaultOptionKey)
        {
            sys.user_settings_add_choice(
                pKey,
                (nuint)key.Length,
                pDescription,
                (nuint)description.Length,
                pDefaultOptionKey,
                (nuint)defaultOptionKey.Length);
        }
    }

    /// <summary>
    ///     Adds a choice option to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="optionKey">
    ///     The key of the option.
    /// </param>
    /// <param name="optionDescription">
    ///     The description of the option.
    /// </param>
    public static void AddChoiceOption(string key, string optionKey, string optionDescription)
    {
        AddChoiceOption(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(optionKey), Encoding.UTF8.GetBytes(optionDescription));
    }

    /// <summary>
    ///     Adds a choice option to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="optionKey">
    ///     The key of the option.
    /// </param>
    /// <param name="optionDescription">
    ///     The description of the option.
    /// </param>
    public static void AddChoiceOption(ReadOnlySpan<byte> key, ReadOnlySpan<byte> optionKey, ReadOnlySpan<byte> optionDescription)
    {
        fixed (byte* pKey = key, pOptionKey = optionKey, pOptionDescription = optionDescription)
        {
            sys.user_settings_add_choice_option(
                pKey,
                (nuint)key.Length,
                pOptionKey,
                (nuint)optionKey.Length,
                pOptionDescription,
                (nuint)optionDescription.Length);
        }
    }

    /// <summary>
    ///     Adds a file select setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    public static void AddFileSelect(string key, string description)
    {
        AddFileSelect(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(description));
    }

    /// <summary>
    ///     Adds a file select setting to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="description">
    ///     The description of the setting.
    /// </param>
    public static void AddFileSelect(ReadOnlySpan<byte> key, ReadOnlySpan<byte> description)
    {
        fixed (byte* pKey = key, pDescription = description)
        {
            sys.user_settings_add_file_select(pKey, (nuint)key.Length, pDescription, (nuint)description.Length);
        }
    }

    /// <summary>
    ///     Adds a file select name filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="pattern">
    ///     The pattern of the filter.
    /// </param>
    public static void AddFileSelectNameFilter(string key, string pattern)
    {
        AddFileSelectNameFilter(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(pattern));
    }

    /// <summary>
    ///     Adds a file select name filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="pattern">
    ///     The pattern of the filter.
    /// </param>
    public static void AddFileSelectNameFilter(ReadOnlySpan<byte> key, ReadOnlySpan<byte> pattern)
    {
        fixed (byte* pKey = key, pPattern = pattern)
        {
            sys.user_settings_add_file_select_name_filter(pKey, (nuint)key.Length, null, 0, pPattern, (nuint)pattern.Length);
        }
    }

    /// <summary>
    ///     Adds a file select name filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="description">
    ///     The description of the filter.
    /// </param>
    /// <param name="pattern">
    ///     The pattern of the filter.
    /// </param>
    public static void AddFileSelectNameFilter(string key, string description, string pattern)
    {
        AddFileSelectNameFilter(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(description), Encoding.UTF8.GetBytes(pattern));
    }

    /// <summary>
    ///     Adds a file select name filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="description">
    ///     The description of the filter.
    /// </param>
    /// <param name="pattern">
    ///     The pattern of the filter.
    /// </param>
    public static void AddFileSelectNameFilter(ReadOnlySpan<byte> key, ReadOnlySpan<byte> description, ReadOnlySpan<byte> pattern)
    {
        fixed (byte* pKey = key, pDescription = description, pPattern = pattern)
        {
            sys.user_settings_add_file_select_name_filter(
                pKey,
                (nuint)key.Length,
                pDescription,
                (nuint)description.Length,
                pPattern,
                (nuint)pattern.Length);
        }
    }

    /// <summary>
    ///     Adds a file select mime filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="mimeType">
    ///     The mime type of the filter.
    /// </param>
    public static void AddFileSelectMimeFilter(string key, string mimeType)
    {
        AddFileSelectMimeFilter(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(mimeType));
    }

    /// <summary>
    ///     Adds a file select mime filter to the user settings.
    /// </summary>
    /// <param name="key">
    ///     The key of the filter.
    /// </param>
    /// <param name="mimeType">
    ///     The mime type of the filter.
    /// </param>
    public static void AddFileSelectMimeFilter(ReadOnlySpan<byte> key, ReadOnlySpan<byte> mimeType)
    {
        fixed (byte* pKey = key, pMimeType = mimeType)
        {
            sys.user_settings_add_file_select_mime_filter(pKey, (nuint)key.Length, pMimeType, (nuint)mimeType.Length);
        }
    }

    /// <summary>
    ///     Sets the tooltip of a setting.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="tooltip">
    ///     The tooltip to set.
    /// </param>
    public static void SetTooltip(string key, string tooltip)
    {
        SetTooltip(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(tooltip));
    }

    /// <summary>
    ///     Sets the tooltip of a setting.
    /// </summary>
    /// <param name="key">
    ///     The key of the setting.
    /// </param>
    /// <param name="tooltip">
    ///     The tooltip to set.
    /// </param>
    public static void SetTooltip(ReadOnlySpan<byte> key, ReadOnlySpan<byte> tooltip)
    {
        fixed (byte* pKey = key, pTooltip = tooltip)
        {
            sys.user_settings_set_tooltip(pKey, (nuint)key.Length, pTooltip, (nuint)tooltip.Length);
        }
    }
}
