using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class ToggleInfo : SettingInfo
{
    public ToggleInfo(IPropertySymbol prop, AttributeData attr)
        : base(attr.GetConstructorArgument<string>(0) ?? prop.Name)
    {
        if (prop.Type.SpecialType != SpecialType.System_Boolean)
        {
            throw new ArgumentException(
                "The property must be a boolean.",
                prop.ToDisplayString());
        }

        Default = attr.GetNamedArgument<bool>("Default");
    }

    public bool Default { get; }

    public override IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
            ("AddBool", [
                ("key",          $"\"{Key}\""),
                ("description",  $"\"{Description}\""),
                ("defaultValue", $"{Default}".ToLower())
            ])
        ];

    public override (string, IEnumerable<(string, string)>) GetValueInfo
        => ("GetToggle", [
            ("key", $"\"{Key}\"")
        ]);
}
