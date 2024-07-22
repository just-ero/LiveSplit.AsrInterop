using System.Collections.Generic;

using LiveSplit.AsrInterop.Core;

namespace LiveSplit.AsrInterop.Settings;

public sealed record FileSelect(
    string Key,
    string Description,
    IEnumerable<string>? MimeFilters = null,
    IEnumerable<(string? Description, string Patterns)>? NameFilters = null) : Setting(Key)
{
    public override void Register()
    {
        UserSettings.AddFileSelect(Key, Description);

        if (MimeFilters != null)
        {
            foreach (string filter in MimeFilters)
            {
                UserSettings.AddFileSelectMimeFilter(Key, filter);
            }
        }

        if (NameFilters != null)
        {
            foreach ((string? description, string patterns) in NameFilters)
            {
                if (description != null)
                {
                    UserSettings.AddFileSelectNameFilter(Key, description, patterns);
                }
                else
                {
                    UserSettings.AddFileSelectNameFilter(Key, patterns);
                }
            }
        }
    }
}
