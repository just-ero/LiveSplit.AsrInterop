using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace LiveSplit.AsrInterop.SourceGenerators.UnitTests;

public static class CSharpIncrementalGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : IIncrementalGenerator, new()
{
    public class Test : CSharpSourceGeneratorTest<TSourceGenerator, DefaultVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net90;

            TestState.AdditionalReferences.Add(typeof(ExternalProcess).Assembly);
            TestState.AdditionalReferences.Add(typeof(AsrInterop.Core.Process).Assembly);
            TestState.AdditionalReferences.Add(typeof(Core.AutosplitterAttribute<>).Assembly);
        }

        protected override ParseOptions CreateParseOptions()
        {
            return ((CSharpParseOptions)base.CreateParseOptions())
                .WithLanguageVersion(LanguageVersion.CSharp12);
        }
    }
}
