using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal sealed class ChoiceData : UserControlData
{
    public ChoiceData(IPropertySymbol prop, AttributeData attr)
        : base(prop, attr)
    {
        EnumType = prop.Type.ToDisplayString();
        Default = attr.GetNamedArgumentOrDefault<string>("Default") ?? $"default({EnumType})";
    }

    public string EnumType { get; }
    public string Default { get; }

    public override IEnumerable<MethodCallData> RegistrationData => [
        ("AddChoice", [
            ("key",              $"\"{Key}\""),
            ("description",      $"\"{Description}\""),
            ("defaultOptionKey", $"\"{Default}\"")
        ]),
        .. OptionsData
    ];

    public override MethodCallData GetValueData
        => ($"GetChoice<{EnumType}>", [
            ("key", $"\"{Key}\"")
        ]);

    private IEnumerable<MethodCallData> OptionsData
    {
        get
        {
            foreach (var @field in ContainingProperty.Type.GetMembers().OfType<IFieldSymbol>())
            {
                string key = @field.ToDisplayString();
                string description = @field.GetAttributes()
                    .Where(a => a.AttributeClass?.ToDisplayString() == "System.ComponentModel.DescriptionAttribute")
                    .Select(a => a.GetConstructorArgumentOrDefault<string>(0))
                    .FirstOrDefault() ?? key;

                yield return ("AddChoiceOption", [
                    ("key",               $"\"{Key}\""),
                    ("optionKey",         $"\"{key}\""),
                    ("optionDescription", $"\"{description}\"")
                ]);
            }
        }
    }

    public override bool Validate(IPropertySymbol prop)
    {
        return prop.Type.SpecialType == SpecialType.System_Enum;
    }
}
