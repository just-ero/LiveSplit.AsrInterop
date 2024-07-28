using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.AutosplitterGenerator;

internal sealed class SplitterInfo(ITypeSymbol target)
{
    public string ClassName { get; } = target.Name;
    public string? Namespace { get; } = target.ContainingNamespace.IsGlobalNamespace
        ? null
        : target.ContainingNamespace.ToString();

    public string FullName => Namespace is null
        ? ClassName
        : $"{Namespace}.{ClassName}";
}
