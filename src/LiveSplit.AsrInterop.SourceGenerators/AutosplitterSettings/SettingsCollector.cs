using System.Collections;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal sealed class SettingsCollector(INamedTypeSymbol type) : IEnumerable<UserSettingData>
{
    public IEnumerator<UserSettingData> GetEnumerator()
    {
        foreach (var prop in type.GetMembers().OfType<IPropertySymbol>())
        {
            if (prop.IsAbstract || prop.SetMethod is not null)
            {
                continue;
            }

            foreach (var attr in prop.GetAttributes())
            {
                switch (attr.AttributeClass?.ToDisplayString())
                {
                    case Names.HeadingAttribute:
                        yield return new HeadingData(prop, attr);
                        break;
                    case Names.H1Attribute:
                        yield return new HeadingData(prop, attr, 0);
                        break;
                    case Names.H2Attribute:
                        yield return new HeadingData(prop, attr, 1);
                        break;
                    case Names.H3Attribute:
                        yield return new HeadingData(prop, attr, 2);
                        break;
                    case Names.H4Attribute:
                        yield return new HeadingData(prop, attr, 3);
                        break;
                    case Names.H5Attribute:
                        yield return new HeadingData(prop, attr, 4);
                        break;
                    case Names.H6Attribute:
                        yield return new HeadingData(prop, attr, 5);
                        break;

                    case Names.ToggleAttribute:
                        yield return new ToggleData(prop, attr);
                        break;

                    case Names.ChoiceAttribute:
                    case Names.ChoiceAttribute_1:
                        yield return new ChoiceData(prop, attr);
                        break;

                    case Names.FileSelectAttribute:
                        yield return new FileSelectData(prop, attr);
                        break;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
