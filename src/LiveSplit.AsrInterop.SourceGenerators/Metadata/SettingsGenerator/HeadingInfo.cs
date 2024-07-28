using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class HeadingInfo(
    string title,
    uint level) : IRegisterSetting
{
    public HeadingInfo(AttributeData attribute)
        : this(
            GetTitle(attribute),
            GetLevel(attribute))
    { }

    public HeadingInfo(AttributeData attribute, uint level)
        : this(
            GetTitle(attribute),
            level)
    { }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegistrationInstructions => [
        ("AddTitle", [
            ("key", $"\"{title}\""),
            ("title", $"\"{title}\""),
            ("level", $"{level}")
        ])
    ];

    private static string GetTitle(AttributeData attribute)
    {
        if (attribute.GetConstructorArgument<string>(0) is not { Length: > 0 } title)
        {
            throw new ArgumentException(
                "The title of the heading must not be empty.");
        }

        return title;
    }

    private static uint GetLevel(AttributeData attribute)
    {
        return attribute.GetConstructorArgument<uint>(1);
    }
}
