using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class OptionInfo : ISettingInfo
{
    public OptionInfo(IFieldSymbol field)
    {
        Key = field.ToString();
        Description = field.GetAttributes()
            .FirstOrDefault(static attr => attr.AttributeClass?.ToString() == "System.ComponentModel.DescriptionAttribute")?
            .GetConstructorArgument<string>(0)
            ?? Key;
    }

    public string Key { get; }
    public string Description { get; }

    public IEnumerable<(string, IEnumerable<(string, object)>)> RegisterCode => [
        ("AddOption", [
            ("key",         Key),
            ("description", Description)
        ])
    ];
}
