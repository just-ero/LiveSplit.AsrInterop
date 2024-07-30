using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class OptionInfo : IRegisterSettingInfo
{
    public OptionInfo(IFieldSymbol field)
    {
        Key = field.ToDisplayString();
        Description = field.GetAttributes()
            .FirstOrDefault(static attr => attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.DescriptionAttribute")?
            .GetConstructorArgument<string>(0)
            ?? Key;
    }

    public string Key { get; }
    public string Description { get; }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
        ("AddOption", [
            ("key",         $"\"{Key}\""),
            ("description", $"\"{Description}\"")
        ])
    ];
}
