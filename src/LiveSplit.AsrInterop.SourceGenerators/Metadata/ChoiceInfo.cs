using System;
using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class ChoiceInfo : IGetSettingInfo
{
    public ChoiceInfo(IPropertySymbol property, AttributeData attribute)
    {
        if (property.Type.SpecialType != SpecialType.System_Enum)
        {
            throw new ArgumentException(
                "The property must be an enum.",
                property.ToDisplayString());
        }

        Key = attribute.GetNamedArgument<string>("Key") ?? property.ToDisplayString();
        Description = attribute.GetNamedArgument<string>("Description") ?? Key;
        Default = attribute.NamedArguments.FirstOrDefault(static arg => arg.Key == "Default").Value.Value?.ToString()
            ?? $"default({property.ToDisplayString()})";

        Options = property.Type.GetMembers().OfType<IFieldSymbol>()
            .Select(field => new OptionInfo(field));

        Property = property;
    }

    public string Key { get; }
    public string Description { get; }
    public string Default { get; }

    public IEnumerable<OptionInfo> Options { get; }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
        ("AddChoice", [
            ("key",          $"\"{Key}\""),
            ("description",  $"\"{Description}\""),
            ("defaultValue", $"{Default}")
        ]),
        .. Options.SelectMany(option => option.RegisterInfo)
    ];

    public IPropertySymbol Property { get; }

    public (string Method, IEnumerable<(string Name, string Value)> Parameters) GetInfo
        => ($"GetChoice<{Property.Type.ToDisplayString()}>", [
            ("key", $"\"{Key}\"")
        ]);
}
