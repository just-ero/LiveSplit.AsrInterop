namespace LiveSplit.AsrInterop.SourceGenerators;

public static class Files
{
    public const string Implementation = $"{Tokens.SplitterFullName}.g.cs";
    public const string AbstractAutosplitter = $"{nameof(LiveSplit)}.{nameof(AsrInterop)}.{nameof(SourceGenerators)}.Autosplitter.g.cs";
}
