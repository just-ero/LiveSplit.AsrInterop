using LiveSplit.AsrInterop;
using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Settings;

namespace LiveSplit.SampleSplitter;

public sealed class SampleSplitter : Autosplitter
{
    public override IAutosplitterSettings Settings { get; } = new SampleSettings();

    public override string[] ProcessNames => ["LiveSplit.exe"];

    public override void Startup()
    {
        Runtime.PrintMessage("Hello, world!");
    }

    public override bool Init(ExternalProcess game)
    {
        return true;
    }

    public override bool Update(ExternalProcess game)
    {
        Runtime.PrintMessage(Settings.GetToggle("Toggle").ToString());

        return true;
    }
}

[GeneratedSettings]
public sealed partial class SampleSettings
{
    [Toggle]
    public partial bool Toggle { get; }
}
