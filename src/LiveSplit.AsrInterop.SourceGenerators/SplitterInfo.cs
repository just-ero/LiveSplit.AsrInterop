using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators;

public sealed class SplitterInfo
{
    public SplitterInfo(ISymbol symbol)
    {
        ClassName = symbol.Name;
        Namespace = symbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : symbol.ContainingNamespace.ToString();
    }

    public string ClassName { get; }
    public string? Namespace { get; }

    public string FullName => Namespace is null ? ClassName : $"{Namespace}.{ClassName}";
}
