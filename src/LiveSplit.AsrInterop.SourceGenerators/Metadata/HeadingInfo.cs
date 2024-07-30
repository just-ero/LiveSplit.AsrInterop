using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class HeadingInfo : IRegisterSettingInfo
{
    public HeadingInfo(AttributeData attribute)
        : this(attribute, attribute.GetNamedArgument<uint>("Level")) { }

    public HeadingInfo(AttributeData attribute, uint level)
    {
        Title = attribute.GetNamedArgument<string>("Title")!;
        Level = level;

        Key = Title;
    }

    public string Key { get; }

    public string Title { get; }
    public uint Level { get; }

    public IEnumerable<(string, IEnumerable<(string, string)>)> RegisterInfo => [
        ("AddTitle", [
            ("key",   $"\"{Key}\""),
            ("title", $"\"{Title}\""),
            ("level", $"{Level}")
        ])
    ];
}
