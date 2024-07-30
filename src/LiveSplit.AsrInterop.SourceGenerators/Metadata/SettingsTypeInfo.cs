using System.Collections.Generic;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class SettingsTypeInfo(INamedTypeSymbol symbol)
{
    public string Name { get; } = symbol.Name;

    public string? Namespace { get; } = symbol.ContainingNamespace.IsGlobalNamespace
        ? null
        : symbol.ContainingNamespace.ToDisplayString();

    public string FullName => Namespace is null
        ? Name
        : $"{Namespace}.{Name}";

    public bool ImplementsInterface => symbol.Implements("LiveSplit.AsrInterop.ISettings");

    public IEnumerable<IRegisterSettingInfo?> SettingInfos
    {
        get
        {
            foreach (var property in symbol.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attribute in property.GetAttributes())
                {
                    string? name = attribute.AttributeClass?.ToDisplayString();
                    yield return name switch
                    {
                        Names.HeadingAttribute => new HeadingInfo(attribute),
                        Names.H1Attribute => new HeadingInfo(attribute, 0),
                        Names.H2Attribute => new HeadingInfo(attribute, 1),
                        Names.H3Attribute => new HeadingInfo(attribute, 2),
                        Names.H4Attribute => new HeadingInfo(attribute, 3),
                        Names.H5Attribute => new HeadingInfo(attribute, 4),
                        Names.H6Attribute => new HeadingInfo(attribute, 5),

                        Names.ToggleAttribute => new ToggleInfo(property, attribute),

                        Names.ChoiceAttribute => new ChoiceInfo(property, attribute),
                        Names.ChoiceAttribute_1 => new ChoiceInfo(property, attribute),

                        _ => null
                    };
                }
            }
        }
    }
}
