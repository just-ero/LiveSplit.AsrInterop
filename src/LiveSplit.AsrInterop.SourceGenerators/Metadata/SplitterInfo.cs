using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal sealed class SplitterInfo(INamedTypeSymbol symbol)
{
    public string ClassName { get; } = symbol.Name;
    public string? Namespace { get; } = symbol.ContainingNamespace.IsGlobalNamespace
        ? null
        : symbol.ContainingNamespace.ToString();

    public string FullName => Namespace is null
        ? ClassName
        : $"{Namespace}.{ClassName}";
}
