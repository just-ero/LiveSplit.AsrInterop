using System.Collections.Generic;
using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal abstract class UserSettingData
{
    protected IPropertySymbol ContainingProperty { get; }
    protected AttributeData Attribute { get; }

    protected UserSettingData(IPropertySymbol prop, AttributeData attr)
    {
        ContainingProperty = prop;
        Attribute = attr;

        Key = attr.GetConstructorArgumentOrDefault<string>(0) ?? prop.ToDisplayString();
    }

    public string Key { get; }

    public abstract IEnumerable<MethodCallData> RegistrationData { get; }

    public string RegistrationCode
    {
        get
        {
            return string.Join(
                "\n",
                RegistrationData.Select(data =>
                {
                    var (method, parameters) = data;

                    var args = string.Join(", ", parameters.Select(p => $"{p.Name}: {p.Value}"));
                    return $"{Names.UserSettings}.{method}({args});";
                }));
        }
    }
}
