using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.AutosplitterSettings.Metadata;

internal sealed class SettingsTypeData(INamedTypeSymbol type)
{
    public bool ImplementsAutosplitterSettings => type.AllInterfaces.Any(i => i.ToDisplayString() == Names.AutosplitterSettingsInterface);

    public string Name { get; } = type.Name;
    public string? Namespace { get; } = type.ContainingNamespace.IsGlobalNamespace ? null : type.ContainingNamespace.ToDisplayString();

    public string FullName => Namespace is null ? Name : $"{Namespace}.{Name}";

    public IEnumerable<UserSettingData> Settings => new SettingsCollector(type);
}
