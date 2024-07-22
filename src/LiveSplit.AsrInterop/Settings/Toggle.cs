using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public sealed record Toggle(
    string Key,
    string Title,
    bool Default) : Setting(Key)
{
    public override void Register()
    {
        UserSettings.AddBool(Key, Title, Default);
    }
}
