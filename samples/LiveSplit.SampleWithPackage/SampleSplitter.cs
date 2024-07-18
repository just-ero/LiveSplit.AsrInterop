using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Settings;

using static LiveSplit.AsrInterop.Settings.SettingsGenerator;

namespace LiveSplit.SampleWithPackage;

public sealed partial class SampleSplitter
{
    public static Setting[] Settings => [
        H1("", [
            H2("")
        ])
    ];

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
