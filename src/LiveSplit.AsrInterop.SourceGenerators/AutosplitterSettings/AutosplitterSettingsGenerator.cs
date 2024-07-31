using System.Text;
using System.Threading;

using LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings.Metadata;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings;

[Generator(LanguageNames.CSharp)]
internal sealed class AutosplitterSettingsGenerator : IIncrementalGenerator
{
    public const string AttributeMetadataName = "LiveSplit.AsrInterop.GeneratedSettingsAttribute";

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

    private static SettingsTypeData Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        return new((INamedTypeSymbol)context.TargetSymbol);
    }

    private static void Generate(SourceProductionContext context, SettingsTypeData data)
    {
        StringBuilder registration = new();
        StringBuilder implementation = new();

        foreach (var setting in data.Settings)
        {
            if (setting is not null)
            {
                registration.AppendLine(setting.RegistrationCode);

                if (setting is UserControlData ctrl)
                {
                    implementation.AppendLine(ctrl.ImplementationCode);
                }
            }
        }

        string code =
            data.Namespace is not null
                ? data.ImplementsAutosplitterSettings
                    ? Sources.Implementation
                    : Sources.ImplementationWithInterface
                : data.ImplementsAutosplitterSettings
                    ? Sources.ImplementationGlobalNamespace
                    : Sources.ImplementationGlobalNamespaceWithInterface;

        code = code
            .Replace(Tokens.TypeNamespace, data.Namespace)
            .Replace(Tokens.TypeName, data.Name)
            .Replace(Tokens.SettingsRegistration, registration.ToString())
            .Replace(Tokens.SettingsImplementation, implementation.ToString());

        context.AddSource($"{data.FullName}.Impl.g.cs", SourceText.From(code, Encoding.UTF8, SourceHashAlgorithm.Sha256));
    }
}
