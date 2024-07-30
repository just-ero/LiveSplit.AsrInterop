using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Text;

using Xunit;

using VerifyCs = LiveSplit.AsrInterop.SourceGenerators.UnitTests.CSharpIncrementalGeneratorVerifier<
    LiveSplit.AsrInterop.SourceGenerators.AutosplitterGenerator>;

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
            [assembly: LiveSplit.AsrInterop.AutosplitterAttribute<{{fullName}}>]

            namespace {{@namespace}};

            public partial class {{@class}} : LiveSplit.AsrInterop.Autosplitter
            {
                public override string[] ProcessNames => [];
            }
            """;

        string exportsFile = Files.AutosplitterExports
            .Replace(Tokens.TypeFullName, fullName);
        string exports = Sources.AutosplitterExports
            .Replace(Tokens.TypeFullName, fullName);

        await new VerifyCs.Test
        {
            TestState =
            {
                Sources = { source },
                GeneratedSources =
                {
                    (
                        typeof(AutosplitterGenerator),
                        exportsFile,
                        SourceText.From(exports, Encoding.UTF8, SourceHashAlgorithm.Sha256)
                    )
                }
            }
        }.RunAsync();
    }
}
