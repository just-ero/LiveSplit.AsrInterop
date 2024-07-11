using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Text;

using Xunit;

using VerifyCs = LiveSplit.AsrInterop.SourceGenerators.UnitTests.CSharpIncrementalGeneratorVerifier<LiveSplit.AsrInterop.SourceGenerators.AutosplitterGenerator>;

namespace LiveSplit.AsrInterop.SourceGenerators.UnitTests;

public sealed class AutosplitterGeneratorTests
{
    [Fact]
    public async Task Should_Generate_Autosplitter()
    {
        string @class = "TestAutosplitter";
        string @namespace = "AutosplitterTests";
        string fullName = $"{@namespace}.{@class}";

        string source = $$"""
            [assembly: LiveSplit.AsrInterop.AutosplitterAttribute<AutosplitterTests.TestAutosplitter>]

            namespace {{@namespace}};

            public partial class {{@class}}
            {
                public override string[] ProcessNames => [];
            }
            """;

        string generatedAutosplitter = Sources.AbstractAutosplitter
            .Replace(Tokens.SplitterFullName, fullName);

        string generatedImplementation = Sources.Implementation
            .Replace(Tokens.SplitterNamespace, @namespace)
            .Replace(Tokens.SplitterClassName, @class);

        await new VerifyCs.Test
        {
            TestState =
            {
                Sources = { source },
                GeneratedSources =
                {
                    (
                        typeof(AutosplitterGenerator),
                        Files.AbstractAutosplitter,
                        SourceText.From(generatedAutosplitter, Encoding.UTF8, SourceHashAlgorithm.Sha256)
                    ),
                    (
                        typeof(AutosplitterGenerator),
                        Files.Implementation.Replace(Tokens.SplitterFullName, fullName),
                        SourceText.From(generatedImplementation, Encoding.UTF8, SourceHashAlgorithm.Sha256)
                    )
                }
            }
        }.RunAsync();
    }
}
