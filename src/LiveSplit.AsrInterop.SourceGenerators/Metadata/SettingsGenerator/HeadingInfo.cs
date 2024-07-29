using System;
using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal sealed class HeadingInfo : ISettingInfo
{
    public HeadingInfo(AttributeData attribute)
    {
        Title = attribute.GetNamedArgument<string>("Title")!;
        Level = attribute.GetNamedArgument<uint>("Level");
    }

    public HeadingInfo(AttributeData attribute, uint level)
    {
        Title = attribute.GetNamedArgument<string>("Title")!;
        Level = level;
    }

    public string Title { get; }
    public uint Level { get; }

    public IEnumerable<(string, IEnumerable<(string, object)>)> RegisterCode => [
        ("AddTitle", [
            ("key",   Title),
            ("title", Title),
            ("level", Level)
        ])
    ];
}
