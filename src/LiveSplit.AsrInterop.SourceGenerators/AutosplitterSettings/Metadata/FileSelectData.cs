using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal sealed class FileSelectData : UserControlData
{
    public FileSelectData(IPropertySymbol prop, AttributeData attr)
        : base(prop, attr) { }

    public override IEnumerable<MethodCallData> RegistrationData => [
        ("AddFileSelect", [
            ("key",         $"\"{Key}\""),
            ("description", $"\"{Description}\"")
        ]),
        .. FiltersData
    ];

    public override MethodCallData GetValueData
        => ("GetFile", [
            ("key", $"\"{Key}\"")
        ]);

    private IEnumerable<MethodCallData> FiltersData
    {
        get
        {
            var nameFilters = Attribute.GetNamedArgumentOrDefault<(string?, string)[]>("NameFilters");
            if (nameFilters is { Length: > 0 })
            {
                foreach (var (description, pattern) in nameFilters)
                {
                    if (description is null)
                    {
                        yield return ("AddFileSelectNameFilter", [
                            ("key",    $"\"{Key}\""),
                            ("pattern", $"\"{pattern}\"")
                        ]);
                    }
                    else
                    {
                        yield return ("AddFileSelectNameFilter", [
                            ("key",         $"\"{Key}\""),
                            ("description", $"\"{description}\""),
                            ("pattern",     $"\"{pattern}\"")
                        ]);
                    }
                }
            }

            var mimeFilters = Attribute.GetNamedArgumentOrDefault<string[]>("MimeFilters");
            if (mimeFilters is { Length: > 0 })
            {
                foreach (var mime in mimeFilters)
                {
                    yield return ("AddFileSelectMimeFilter", [
                        ("key",      $"\"{Key}\""),
                        ("mimeType", $"\"{mime}\"")
                    ]);
                }
            }
        }
    }

    public override bool Validate(IPropertySymbol prop)
    {
        return prop.Type.SpecialType == SpecialType.System_String;
    }
}
