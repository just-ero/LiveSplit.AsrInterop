using System;

namespace LiveSplit.AsrInterop.Settings;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ToggleAttribute : Attribute
{
    public ToggleAttribute() { }

    public ToggleAttribute(string key)
    {
        Key = key;
        Description = key;
    }

    public string? Key { get; }
    public string? Description { get; set; }

    public bool Default { get; set; }
}
