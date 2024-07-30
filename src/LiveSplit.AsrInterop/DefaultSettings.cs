using LiveSplit.AsrInterop.Core;
using LiveSplit.AsrInterop.Settings;

namespace LiveSplit.AsrInterop;

public sealed class DefaultSettings : IAutosplitterSettings
{
    public SettingsMap Map { get; set; }

    public void RegisterSettings() { }
}
