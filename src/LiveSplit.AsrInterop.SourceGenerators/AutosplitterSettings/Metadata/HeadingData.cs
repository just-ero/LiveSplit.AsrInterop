using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal sealed class HeadingData : UserSettingData
{
    public HeadingData(IPropertySymbol prop, AttributeData attr)
        : this(prop, attr, attr.GetConstructorArgumentOrDefault<uint>(1)) { }

    public HeadingData(IPropertySymbol prop, AttributeData attr, uint level)
        : base(prop, attr)
    {
        Title = Key;
        Level = level;
    }

    public string Title { get; }
    public uint Level { get; }

    public override IEnumerable<MethodCallData> RegistrationData => [
        ("AddTitle", [
            ("key",   $"\"{Key}\""),
            ("title", $"\"{Title}\""),
            ("level", $"{Level}")
        ])
    ];
}
