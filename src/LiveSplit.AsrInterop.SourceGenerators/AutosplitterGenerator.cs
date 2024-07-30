using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LiveSplit.AsrInterop.SourceGenerators;

[Generator(LanguageNames.CSharp)]
internal sealed class AutosplitterGenerator : IIncrementalGenerator
{
    private const string AttributeMetadataName = "LiveSplit.AsrInterop.SourceGenerators.Core.AutosplitterAttribute`1";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var data = context.SyntaxProvider
            .ForAttributeWithMetadataName(AttributeMetadataName, Include, Transform);

        context.RegisterSourceOutput(data, Generate);
    }

    private static bool Include(SyntaxNode node, CancellationToken ct)
    {
        return true;
    }

    private static Metadata.AutosplitterTypeInfo Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        // Gets the class passed as a type argument to the attribute.
        ITypeSymbol targetType = context.Attributes[0].AttributeClass!.TypeArguments[0];

        return new((INamedTypeSymbol)targetType);
    }

    private static void Generate(SourceProductionContext context, Metadata.AutosplitterTypeInfo data)
    {
        string exportsFile = Files.AutosplitterExports
            .Replace(Tokens.TypeFullName, data.FullName);
        string exports = Sources.AutosplitterExports
            .Replace(Tokens.TypeFullName, data.FullName);

        context.AddSource(exportsFile, SourceText.From(exports, Encoding.UTF8, SourceHashAlgorithm.Sha256));
    }
}
