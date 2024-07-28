using System.Collections.Generic;

namespace LiveSplit.AsrInterop.SourceGenerators.Metadata.SettingsGenerator;

internal interface IRegisterSetting
{
    IEnumerable<(string Method, IEnumerable<(string Name, string Value)> Arguments)> RegistrationInstructions { get; }
}
