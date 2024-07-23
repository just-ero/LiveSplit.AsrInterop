using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public sealed class SettingsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var infos = context.SyntaxProvider
            .ForAttributeWithMetadataName("LiveSplit.AsrInterop.SourceGenerators.Core.SettingAttribute", Filter, GetProperties)
            .Collect();
    }

    private static bool Filter(SyntaxNode node, CancellationToken ct)
    {
        return true;
    }

    private static IEnumerable<IPropertySymbol> GetProperties(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        if (context.TargetSymbol is not INamedTypeSymbol symbol)
        {
            return [];
        }

        return symbol.GetMembers().OfType<IPropertySymbol>()
            .SelectMany(static (property, _) => property.GetAttributes())
            .Where(static attribute => attribute.AttributeClass?.Name == "SettingAttribute")
    }

    private sealed class SettingsInfo
    {
        public SettingsInfo(INamedTypeSymbol symbol)
        {
            ClassName = symbol.Name;
            Namespace = symbol.ContainingNamespace.IsGlobalNamespace
                ? null
                : symbol.ContainingNamespace.ToString();
        }

        public string ClassName { get; }
        public string? Namespace { get; }

        public string FullName => Namespace is null
            ? ClassName
            : $"{Namespace}.{ClassName}";
    }

    private sealed class SettingInfo
    {
        public SettingInfo(IPropertySymbol symbol)
        {
            FullDefinition = symbol.ToString();
        }

        public string Name { get; }
    }
}
