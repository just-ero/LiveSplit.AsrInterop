using System.Collections.Generic;
using System.ComponentModel;

using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Settings;

using static LiveSplit.AsrInterop.Settings.SettingsHelper;

namespace LiveSplit.SampleSplitter;

public enum Choice
{
    [Description("Option 1 Description")]
    Option1,
    Option2,
    Option3
}

public sealed partial class SampleSplitter
{
    public override string[] ProcessNames => [];

    public override IEnumerable<Setting> Settings => [
        H1("Title 1", [
            Toggle("Toggle 1").Tooltip("Tooltip 1"),
            Toggle("Toggle 2"),
            H2("Title 2", [
                Toggle("Toggle 3"),
                Choice<Choice>("Choice 1", Choice.Option2),
                FileSelect("", "").Tooltip("Tooltip 2")
                    .Filters("Filter 1", "*.ext1", "*.ext2")
            ]),
        ])
    ];

    public override void Startup()
    {
        Runtime.PrintMessage("Hello, world!");
    }

    public override bool Init(Process game)
    {
        return true;
    }
}
