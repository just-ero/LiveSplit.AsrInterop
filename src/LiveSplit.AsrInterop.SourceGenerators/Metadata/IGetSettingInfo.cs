using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata;

internal interface IGetSettingInfo : IRegisterSettingInfo
{
    IPropertySymbol Property { get; }

    (string Method, IEnumerable<(string Name, string Value)> Parameters) GetInfo { get; }
}
