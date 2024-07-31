using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

internal abstract class UserControlData : UserSettingData
{
    protected UserControlData(IPropertySymbol prop, AttributeData attr)
        : base(prop, attr)
    {
        Description = attr.GetNamedArgumentOrDefault<string>("Description") ?? prop.Name;
        Tooltip = attr.GetNamedArgumentOrDefault<string>("Tooltip");
    }

    public string Description { get; }
    public string? Tooltip { get; }

    public abstract MethodCallData GetValueData { get; }

    public abstract bool Validate(IPropertySymbol prop);

    public string ImplementationCode
    {
        get
        {
            string modifiers = ContainingProperty.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                Accessibility.ProtectedOrInternal => "protected internal",
                Accessibility.ProtectedAndInternal => "private protected",
                Accessibility.Protected => "protected",
                _ => "private"
            };

            if (ContainingProperty.IsStatic)
            {
                modifiers += " static";
            }
            else
            {
                if (ContainingProperty.IsRequired)
                {
                    modifiers += " required";
                }

                if (ContainingProperty.IsVirtual)
                {
                    modifiers += " virtual";
                }
                else if (ContainingProperty.IsOverride)
                {
                    modifiers += " override";
                }
                else if (ContainingProperty.IsSealed)
                {
                    modifiers += " sealed";
                }
            }

            var (method, parameters) = GetValueData;
            var args = string.Join(", ", parameters.Select(p => $"{p.Name}: {p.Value}"));

            return $"{modifiers} partial {ContainingProperty.Type.ToDisplayString()} {ContainingProperty.Name} => (({Names.AutosplitterSettingsInterface})this).{method}({args});";
        }
    }
}
