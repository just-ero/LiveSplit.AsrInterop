using System;
using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class ChoiceInfo(
    string key,
    string? description,
    string defaultValue,
    IPropertySymbol property) : IRegisterSetting, IGetSetting
{
    public ChoiceInfo(AttributeData attribute, IPropertySymbol property)
        : this(
            GetKey(attribute),
            GetDescription(attribute),
            GetDefaultValue(attribute, property),
            property)
    {
        if (property.Type.TypeKind != TypeKind.Enum)
        {
            throw new ArgumentException(
                "The property must be an enum.",
                property.MetadataName);
        }
    }

    public string ImplementationMethod => $"""
        TryGetValue("{key}", out string name)
            ? Enum.Parse<{property.MetadataName}>(name)
            : {defaultValue}
        """;

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegistrationInstructions => [
        ("AddChoice", [
            ("key", $"\"{key}\""),
            ("description", $"\"{description ?? key}\""),
            ("defaultValue", $"{defaultValue}.ToString()")
        ]),
        .. Options
    ];

    private IEnumerable<(string, IEnumerable<(string, string)>)> Options
    {
        get
        {
            foreach (var @field in property.Type.GetMembers().OfType<IFieldSymbol>())
            {
                string key = @field.MetadataName;
                string description = @field.GetAttributes()
                    .FirstOrDefault(static attr => attr.AttributeClass?.MetadataName == "System.ComponentModel.DescriptionAttribute")?
                    .GetConstructorArgument<string>(0)
                    ?? key;

                yield return ("AddChoiceOption", [
                    ("key", $"{key}"),
                    ("description", $"{description}")
                ]);
            }
        }
    }

    private static string GetKey(AttributeData attribute)
    {
        if (attribute.GetConstructorArgument<string>(0) is not { Length: > 0 } key)
        {
            throw new ArgumentException(
                "The key of the choice must not be empty.");
        }

        return key;
    }

    private static string? GetDescription(AttributeData attribute)
    {
        return attribute.GetConstructorArgument<string>(1);
    }

    private static string GetDefaultValue(AttributeData attribute, IPropertySymbol property)
    {
        var defaultValue = attribute.NamedArguments.FirstOrDefault(static arg => arg.Key == "Default").Value;
        if (!SymbolEqualityComparer.Default.Equals(defaultValue.Type, property.Type))
        {
            throw new ArgumentException(
                "The default value must be of the same type as the property.",
                property.ToString());
        }

        return defaultValue.Value?.ToString()
            ?? $"default({property})";
    }
}
