using System.Collections.Generic;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public sealed record Heading(
    string Title,
    HeadingLevel Level,
    IEnumerable<Setting>? Children = null) : Setting(Title)
{
    public override void Register()
    {
        UserSettings.AddTitle(Title, Title, (uint)Level);

        if (Children != null)
        {
            foreach (Setting child in Children)
            {
                child.Register();
            }
        }
    }
}
