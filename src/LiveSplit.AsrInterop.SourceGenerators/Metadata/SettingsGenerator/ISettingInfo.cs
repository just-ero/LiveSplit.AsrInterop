using System.Collections.Generic;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal interface ISettingInfo
{
    IEnumerable<(string Method, IEnumerable<(string Name, object Value)> Arguments)> RegisterCode { get; }
}
