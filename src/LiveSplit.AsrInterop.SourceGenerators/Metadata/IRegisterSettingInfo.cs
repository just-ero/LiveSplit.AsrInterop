using System.Collections.Generic;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal interface IRegisterSettingInfo
{
    string Key { get; }

    IEnumerable<(string Method, IEnumerable<(string Name, string Value)> Parameters)> RegisterInfo { get; }
}
