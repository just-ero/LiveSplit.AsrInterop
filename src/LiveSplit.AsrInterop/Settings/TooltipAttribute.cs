using System;

namespace LiveSplit.AsrInterop.Settings;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class TooltipAttribute : Attribute
{
    public TooltipAttribute(string text)
    {
        Text = text;
    }

    public string Text { get; }
}
