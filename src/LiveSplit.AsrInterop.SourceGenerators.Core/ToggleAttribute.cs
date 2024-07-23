using System;

namespace LiveSplit.AsrInterop.SourceGenerators.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public abstract class SettingAttribute : Attribute
{
    public SettingAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ToggleAttribute : SettingAttribute
{
    public ToggleAttribute(string key)
        : this(key, key) { }

    public ToggleAttribute(string key, string description)
        : base(key)
    {
        Description = description;
    }

    public string Description { get; set; }
    public bool Default { get; set; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ChoiceAttribute<TEnum> : SettingAttribute
    where TEnum : unmanaged, Enum
{
    public ChoiceAttribute(string key)
        : this(key, key) { }

    public ChoiceAttribute(string key, string description)
        : base(key)
    {
        Description = description;
    }

    public string Description { get; set; }
    public TEnum Default { get; set; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class TooltipAttribute : Attribute
{
    public TooltipAttribute(string text)
    {
        Text = text;
    }

    public string Text { get; }
}
