using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class ToggleInfo(string key, string? description, bool defaultValue) : IRegisterSetting, IGetSetting
{
    public ToggleInfo(AttributeData attribute, IPropertySymbol property)
        : this(
            GetKey(attribute),
            GetDescription(attribute),
            GetDefaultValue(attribute))
    {
        if (property.Type.SpecialType != SpecialType.System_Boolean)
        {
            throw new ArgumentException(
                "The property must be a boolean.",
                property.MetadataName);
        }
    }

    public string ImplementationMethod => $"""
        TryGetValue("{key}", out bool value) && value
        """;

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegistrationInstructions => [
        ("AddBool", [
            ("key", $"\"{key}\""),
            ("description",$"\"{description ?? key}\""),
            ("defaultValue", $"{defaultValue}".ToLowerInvariant())
        ])
    ];

    private static string GetKey(AttributeData attribute)
    {
        if (attribute.GetConstructorArgument<string>(0) is not { Length: > 0 } key)
        {
            throw new ArgumentException(
                "The key of the toggle must not be empty.");
        }

        return key;
    }

    private static string? GetDescription(AttributeData attribute)
    {
        return attribute.GetConstructorArgument<string>(1);
    }

    private static bool GetDefaultValue(AttributeData attribute)
    {
        return attribute.GetNamedArgument<bool>("Default");
    }
}
