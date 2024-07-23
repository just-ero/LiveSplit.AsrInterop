using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace LiveSplit.AsrInterop.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public sealed class AutosplitterGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var infos = context.SyntaxProvider
            .ForAttributeWithMetadataName("LiveSplit.AsrInterop.SourceGenerators.Core.AutosplitterAttribute`1", TargetIsAssembly, GetInfo);

        context.RegisterSourceOutput(infos, Generate);
    }

    private static bool TargetIsAssembly(SyntaxNode node, CancellationToken ct)
    {
        return node is CompilationUnitSyntax;
    }

    private static SplitterInfo GetInfo(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        return new(context.Attributes[0].AttributeClass!.TypeArguments[0]);
    }

    private static void Generate(SourceProductionContext context, SplitterInfo info)
    {
        string autosplitter = Sources.AbstractAutosplitter
            .Replace(Tokens.SplitterFullName, info.FullName);

        string implementation = info.Namespace is null
            ? Sources.GlobalNamespaceImplementation
                .Replace(Tokens.SplitterClassName, info.ClassName)
            : Sources.Implementation
                .Replace(Tokens.SplitterNamespace, info.Namespace)
                .Replace(Tokens.SplitterClassName, info.ClassName);

        string autosplitterFileName = Files.AbstractAutosplitter;
        string implementationFileName = Files.Implementation
            .Replace(Tokens.SplitterFullName, info.FullName);

        context.AddSource(autosplitterFileName, SourceText.From(autosplitter, Encoding.UTF8, SourceHashAlgorithm.Sha256));
        context.AddSource(implementationFileName, SourceText.From(implementation, Encoding.UTF8, SourceHashAlgorithm.Sha256));
    }

    private sealed class SplitterInfo
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
}
