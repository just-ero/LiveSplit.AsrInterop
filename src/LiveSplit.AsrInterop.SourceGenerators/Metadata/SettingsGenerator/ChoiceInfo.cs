using System;
using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class ChoiceInfo : ISettingInfo
{
    public ChoiceInfo(AttributeData attribute, IPropertySymbol property)
    {
        if (property.Type.SpecialType != SpecialType.System_Enum)
        {
            throw new ArgumentException(
                "The property must be an enum.",
                property.ToString());
        }

        Key = attribute.GetNamedArgument<string>("Key") ?? property.ToString();
        Description = attribute.GetNamedArgument<string>("Description") ?? Key;
        Default = attribute.NamedArguments.FirstOrDefault(static arg => arg.Key == "Default").Value.Value?.ToString()
            ?? $"default({property})";

        Options = property.Type.GetMembers().OfType<IFieldSymbol>()
            .Select(field => new OptionInfo(field));
    }

    public string Key { get; }
    public string Description { get; }
    public string Default { get; }

    public IEnumerable<OptionInfo> Options { get; }

    public IEnumerable<(string, IEnumerable<(string, object)>)> RegisterCode => [
        ("AddChoice", [
            ("key",          Key),
            ("description",  Description),
            ("defaultValue", Default)
        ]),
        .. Options.SelectMany(option => option.RegisterCode)
    ];
}
