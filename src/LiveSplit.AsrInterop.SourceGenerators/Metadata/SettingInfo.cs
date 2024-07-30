using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal abstract class SettingInfo
{
    public virtual IEnumerable<(string Method, IEnumerable<(string Name, string Value)> Parameters)> RegisterInfo { get; } = [];
    public virtual (string Method, IEnumerable<(string Name, string Value)> Parameters) GetValueInfo { get; } = default;

    private static string? FindKey(AttributeData attribute)
    {
        return attribute.ConstructorArguments.Length switch
        {
            0 => null,
            _ => attribute.GetConstructorArgument<string>(0)
        };
    }

    private static string? FindDescription(ImmutableArray<AttributeData> attributes)
    {
        return attributes
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == "System.ComponentModel.DescriptionAttribute")?
            .GetConstructorArgument<string>(0);
    }

    public static IEnumerable<IEnumerable<SettingInfo>> SettingsForType(INamedTypeSymbol type)
    {
        foreach (var prop in type.GetMembers().OfType<IPropertySymbol>())
        {
            yield return SettingsForProperty(prop);
        }
    }

    private static IEnumerable<SettingInfo> SettingsForProperty(IPropertySymbol prop)
    {
        ImmutableArray<AttributeData> attrs = prop.GetAttributes();
        if (attrs.Length == 0)
        {
            yield break;
        }

        // string? key = null, description = null, tooltip = null;

        var headings = GetHeadings(prop, attrs);

        var description = FindDescription(attrs);

    }

    private static IEnumerable<HeadingInfo> GetHeadings(IPropertySymbol prop, ImmutableArray<AttributeData> attrs)
    {
        foreach (AttributeData attr in attrs)
        {
            switch (attr.AttributeClass?.ToDisplayString())
            {
                case Names.HeadingAttribute:
                    yield return new(prop, attr);
                    break;
                case Names.H1Attribute:
                    yield return new(prop, attr, 0);
                    break;
                case Names.H2Attribute:
                    yield return new(prop, attr, 1);
                    break;
                case Names.H3Attribute:
                    yield return new(prop, attr, 2);
                    break;
                case Names.H4Attribute:
                    yield return new(prop, attr, 3);
                    break;
                case Names.H5Attribute:
                    yield return new(prop, attr, 4);
                    break;
                case Names.H6Attribute:
                    yield return new(prop, attr, 5);
                    break;
            }
        }
    }

    private static SettingInfo? Create(IPropertySymbol prop, AttributeData attribute)
    {
        if (attribute.AttributeClass?.ToDisplayString() == Names.ToggleAttribute)
        {
            string key = FindKey(attribute) ?? prop.Name;
            string description = FindDescription(prop.GetAttributes()) ?? key;
        }

        return (attribute.AttributeClass?.ToDisplayString()) switch
        {
            Names.ToggleAttribute => new ToggleInfo(prop, attribute),
            _ => null
        };
    }
}
