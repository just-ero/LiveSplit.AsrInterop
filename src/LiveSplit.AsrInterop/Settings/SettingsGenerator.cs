using System.ComponentModel.DataAnnotations;

namespace LiveSplit.AsrInterop.Settings;

public abstract record Setting(
    string Key);

internal abstract record Title(
    string Text,
    int Level,
    Setting[]? Children = null) : Setting(Text);

internal sealed record H1(
    string Text,
    Setting[]? Children = null) : Title(Text, 1, Children);

internal sealed record H2(
    string Text,
    Setting[]? Children = null) : Title(Text, 2, Children);

internal sealed record H3(
    string Text,
    Setting[]? Children = null) : Title(Text, 3, Children);

internal sealed record H4(
    string Text,
    Setting[]? Children = null) : Title(Text, 4, Children);

internal sealed record H5(
    string Text,
    Setting[]? Children = null) : Title(Text, 5, Children);

internal sealed record H6(
    string Text,
    Setting[]? Children = null) : Title(Text, 6, Children);

public static class SettingsGenerator
{
    public static Setting H1(string text, params Setting[] children)
    {
        return new H1(text, children);
    }

    public static Setting H2(string text, params Setting[] children)
    {
        return new H2(text, children);
    }

    public static Setting H3(string text, params Setting[] children)
    {
        return new H3(text, children);
    }

    public static Setting H4(string text, params Setting[] children)
    {
        return new H4(text, children);
    }

    public static Setting H5(string text, params Setting[] children)
    {
        return new H5(text, children);
    }

    public static Setting H6(string text, params Setting[] children)
    {
        return new H6(text, children);
    }
}
