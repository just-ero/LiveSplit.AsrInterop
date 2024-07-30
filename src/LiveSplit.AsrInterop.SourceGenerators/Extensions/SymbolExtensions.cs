using System.Linq;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Extensions;

internal static class SymbolExtensions
{
    public static bool Implements(this INamedTypeSymbol symbol, string interfaceMetadataName)
    {
        return symbol.AllInterfaces.Any(i => i.ToDisplayString() == interfaceMetadataName);
    }
}
