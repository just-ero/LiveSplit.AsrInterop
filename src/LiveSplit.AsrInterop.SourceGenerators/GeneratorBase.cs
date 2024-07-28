using System.Collections.Generic;
using System.Text;
using System.Threading;

using LiveSplit.AsrInterop.SourceGenerators.Metadata.AutosplitterGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LiveSplit.AsrInterop.SourceGenerators;

internal abstract class IncrementalGenerator<T> : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var infos = context.SyntaxProvider
            .ForAttributeWithMetadataName(AttributeMetadataName, Include, Transform);

        context.RegisterSourceOutput(infos, Generate);
    }

    protected abstract string AttributeMetadataName { get; }

    protected abstract bool Include(SyntaxNode node, CancellationToken ct);
    protected abstract T Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct);

    protected abstract IEnumerable<(string FileName, string Code)> GetSourceOutput(SourceProductionContext context, T info);

    private void Generate(SourceProductionContext context, T info)
    {
        foreach (var (fileName, code) in GetSourceOutput(context, info))
        {
            context.AddSource(fileName, SourceText.From(code, Encoding.UTF8, SourceHashAlgorithm.Sha256));
        }
    }
}
