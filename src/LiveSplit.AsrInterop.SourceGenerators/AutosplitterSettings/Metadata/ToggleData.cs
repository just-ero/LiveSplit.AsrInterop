using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal sealed class ToggleData : UserControlData
{
    public ToggleData(IPropertySymbol prop, AttributeData attr)
        : base(prop, attr)
    {
        Default = attr.GetNamedArgumentOrDefault<bool>("Default");
    }

    public bool Default { get; }

    public override IEnumerable<MethodCallData> RegistrationData => [
        ("AddBool", [
            ("key",          $"\"{Key}\""),
            ("description",  $"\"{Description}\""),
            ("defaultValue", $"{Default}".ToLower())
        ])
    ];

    public override MethodCallData GetValueData
        => ("GetToggle", [
            ("key", $"\"{Key}\"")
        ]);

    public override bool Validate(IPropertySymbol prop)
    {
        return prop.Type.SpecialType == SpecialType.System_Boolean;
    }
}
