using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class FileSelectInfo(
    string key,
    string? description,
    IPropertySymbol property) : IRegisterSetting, IGetSetting
{
    public FileSelectInfo(AttributeData attribute, IPropertySymbol property)
        : this(
            GetKey(attribute),
            GetDescription(attribute),
            property)
    {
        if (property.Type.SpecialType != SpecialType.System_String)
        {
            throw new ArgumentException(
                "The property must be a string.",
                property.MetadataName);
        }
    }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegistrationInstructions => [
        ("AddFileSelect", [
            ("key", $"{key}"),
            ("description", $"{description ?? key}")
        ]),
        .. Filters
    ];

    private IEnumerable<(string, IEnumerable<(string, string)>)> Filters
    {
        get
        {
            foreach (var attribute in property.GetAttributes())
            {
                if (attribute.AttributeClass?.MetadataName == Names.SettingsGenerator.FilterAttribute)
                {
                    if (attribute.ConstructorArguments.Length == 1)
                    {
                        yield return ("AddFileSelectMimeFilter", [
                            ("key", $"{key}"),
                            ("mimeType", $"{attribute.GetConstructorArgument<string>(0)}")
                        ]);
                    }
                    else if (attribute.ConstructorArguments.Length == 2)
                    {
                        if (attribute.GetConstructorArgument<string>(1) is { } description)
                        {
                            yield return ("AddFileSelectNameFilter", [
                                ("key", $"{key}"),
                                ("description", $"{description}"),
                                ("pattern", $"{attribute.GetConstructorArgument<string>(0)}")
                            ]);
                        }
                        else
                        {
                            yield return ("AddFileSelectNameFilter", [
                                ("key", $"{key}"),
                                ("pattern", $"{attribute.GetConstructorArgument<string>(0)}")
                            ]);
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException(
                            "The filter attribute must have one or two arguments.");
                    }
                }
            }
        }
    }

    public string ImplementationMethod => throw new System.NotImplementedException();

    public static IRegisterSetting? FromAttribute(AttributeData attribute, IPropertySymbol property)
    {
        if (property.Type.SpecialType != SpecialType.System_String)
        {
            throw new System.ArgumentException(
                "The property must be a string.");
        }

        return attribute.AttributeClass?.MetadataName switch
        {
            Names.SettingsGenerator.FileSelectAttribute => new FileSelectInfo(GetKey(attribute), GetDescription(attribute), property),
            _ => null
        };
    }

    private static string GetKey(AttributeData attribute)
    {
        if (attribute.GetConstructorArgument<string>(0) is not { Length: > 0 } key)
        {
            throw new System.ArgumentException(
                "The key of the file select must not be empty.");
        }

        return key;
    }

    private static string? GetDescription(AttributeData attribute)
    {
        return attribute.GetConstructorArgument<string>(1);
    }
}
