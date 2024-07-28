using System.Collections.Generic;
using System.Threading;

using LiveSplit.AsrInterop.SourceGenerators.Metadata.AutosplitterGenerator;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators;

[Generator(LanguageNames.CSharp)]
internal sealed class AutosplitterGenerator : IncrementalGenerator<SplitterInfo>
{
    protected override string AttributeMetadataName { get; } = "LiveSplit.AsrInterop.SourceGenerators.Core.AutosplitterAttribute`1";

    protected override bool Include(SyntaxNode node, CancellationToken ct)
    {
        return true;
    }

    protected override SplitterInfo Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        // Gets the class passed as a type argument to the attribute.
        ITypeSymbol targetType = context.Attributes[0].AttributeClass!.TypeArguments[0];

        return new(targetType);
    }

    protected override IEnumerable<(string, string)> GetSourceOutput(SourceProductionContext context, SplitterInfo info)
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

        return [
            (autosplitterFileName, autosplitter),
            (implementationFileName, implementation)
        ];
    }
}
