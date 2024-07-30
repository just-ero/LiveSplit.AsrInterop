using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class ToggleInfo : IGetSettingInfo
{
    public ToggleInfo(IPropertySymbol property, AttributeData attribute)
    {
        if (property.Type.SpecialType != SpecialType.System_Boolean)
        {
            throw new ArgumentException(
                "The property must be a boolean.",
                property.ToDisplayString());
        }

        Key = attribute.GetNamedArgument<string>("Key") ?? property.ToDisplayString();
        Description = attribute.GetNamedArgument<string>("Description") ?? Key;
        Default = attribute.GetNamedArgument<bool>("Default");

        Property = property;
    }

    public string Key { get; }
    public string Description { get; }
    public bool Default { get; }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
            ("AddBool", [
            ("key",          $"\"{Key}\""),
            ("description",  $"\"{Description}\""),
            ("defaultValue", $"{Default}".ToLower())])];

    public IPropertySymbol Property { get; }

    public (string Method, IEnumerable<(string Name, string Value)> Parameters) GetInfo
        => ("GetToggle", [
            ("key", $"\"{Key}\"")
        ]);
}
