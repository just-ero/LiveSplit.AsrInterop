using System.Text;
using System.Threading;

using LiveSplit.AsrInterop.SourceGenerators.Extensions;
using LiveSplit.AsrInterop.SourceGenerators.Metadata;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators;

[Generator(LanguageNames.CSharp)]
internal sealed class SettingsGenerator : IIncrementalGenerator
{
    private const string AttributeMetadataName = "LiveSplit.AsrInterop.GeneratedSettingsAttribute";

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

    private static SettingsTypeInfo Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        return new((INamedTypeSymbol)context.TargetSymbol);
    }

    private static void Generate(SourceProductionContext context, SettingsTypeInfo data)
    {
        string code = data.Namespace is not null
            ? data.ImplementsInterface
                ? Sources.SettingsImplementationNoInterface
                : Sources.SettingsImplementation
            : data.ImplementsInterface
                ? Sources.SettingsImplementationGlobalNamespaceNoInterface
                : Sources.SettingsImplementationGlobalNamespace;

        StringBuilder registration = new();
        StringBuilder implementation = new();

        foreach (var foo in data.SettingInfos)
        {
            if (foo is not null)
            {
                registration.AppendLine(foo.FormatRegisterInfo());

                if (foo is IGetSettingInfo get)
                {
                    implementation.AppendLine(get.FormatPartialImplementation());
                }
            }
        }

        code = code.Replace(Tokens.SettingsRegistration, registration.ToString());
        code = code.Replace(Tokens.SettingsImplementation, implementation.ToString());

        context.AddSource($"{data.FullName}.Impl.g.cs", code);
    }
}
