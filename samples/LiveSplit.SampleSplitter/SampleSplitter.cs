using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Extensions;

namespace LiveSplit.SampleSplitter;

public sealed partial class SampleSplitter
{
    public override string[] ProcessNames => ["Refunct-Win32-Shipping.exe"];

    public override void Startup()
    {
        Runtime.PrintMessage("Hello, world!"u8);
    }

    public override void Init(AsrInterop.Process game)
    {
        var buttons = game.Read<int>(game.MainModule.BaseAddress + 0x1FBF9EC, 0xC0, 0xA8);
        Runtime.PrintMessage($"Buttons: {buttons}");
    }
}
