using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class AutosplitterTypeInfo(INamedTypeSymbol symbol)
{
    public string Name { get; } = symbol.Name;

    public string? Namespace { get; } = symbol.ContainingNamespace.IsGlobalNamespace
        ? null
        : symbol.ContainingNamespace.ToDisplayString();

    public string FullName => Namespace is null
        ? Name
        : $"{Namespace}.{Name}";
}
