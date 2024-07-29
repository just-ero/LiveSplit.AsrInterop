using System;

namespace LiveSplit.AsrInterop.Settings;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ChoiceAttribute<TEnum> : Attribute
    where TEnum : unmanaged, Enum
{
    public ChoiceAttribute() { }

    public ChoiceAttribute(string key)
    {
        Key = key;
    }

    public string? Key { get; }
    public string? Description { get; set; }

    public TEnum Default { get; set; }
}
