using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class HeadingInfo : SettingInfo
{
    // HeadingAttribute
    public HeadingInfo(IPropertySymbol prop, AttributeData attr)
        : this(prop, attr, attr.GetConstructorArgument<uint>(1)) { }

    // H1Attribute, H2Attribute, H3Attribute, H4Attribute, H5Attribute, H6Attribute
    public HeadingInfo(IPropertySymbol prop, AttributeData attr, uint level)
        : base(attr.GetConstructorArgument<string>(0) ?? prop.Name)
    {
        Title = Key;
        Level = level;
    }

    public string Title { get; }
    public uint Level { get; }

    public override IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
        ("AddTitle", [
            ("key",   $"\"{Key}\""),
            ("title", $"\"{Title}\""),
            ("level", $"{Level}")
        ])
    ];
}
