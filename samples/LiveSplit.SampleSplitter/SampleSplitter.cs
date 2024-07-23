using LiveSplit.AsrInterop.Core;

namespace LiveSplit.SampleSplitter;

public sealed partial class SampleSplitter
{
    public override string[] ProcessNames => [];

    public override void Startup()
    {
        Runtime.PrintMessage("Hello, world!");
    }

    public override bool Init(Process game)
    {
        return true;
    }
}
