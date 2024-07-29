using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class ToggleInfo : ISettingInfo
{
    public ToggleInfo(AttributeData attribute, IPropertySymbol property)
    {
        if (property.Type.SpecialType != SpecialType.System_Boolean)
        {
            throw new ArgumentException(
                "The property must be a boolean.",
                property.ToString());
        }

        Key = attribute.GetNamedArgument<string>("Key") ?? property.ToString();
        Description = attribute.GetNamedArgument<string>("Description") ?? Key;
        Default = attribute.GetNamedArgument<bool>("Default");
    }

    public string Key { get; }
    public string Description { get; }
    public bool Default { get; }

    public IEnumerable<(string, IEnumerable<(string, object)>)> RegisterCode => [
        ("AddBool", [
            ("key",          Key),
            ("description",  Description),
            ("defaultValue", Default)
        ])
    ];
}
