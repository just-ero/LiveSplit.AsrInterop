using System.Linq;

using LiveSplit.AsrInterop.SourceGenerators.Metadata;

namespace LiveSplit.AsrInterop.SourceGenerators.Extensions;

internal static class SettingInfoExtensions
{
    public static string FormatRegisterInfo(this IRegisterSettingInfo setting)
    {
        return string.Join("\n", setting.RegisterInfo.Select(info =>
        {
            string args = string.Join(", ", info.Parameters.Select(p => $"{p.Name}: {p.Value}"));
            return $"LiveSplit.AsrInterop.Core.UserSettings.{info.Method}({args});";
        }));
    }

    public static string? FormatPartialImplementation(this IGetSettingInfo setting)
    {
        string decl = setting.Property.ToDisplayString();
        string args = string.Join(", ", setting.GetInfo.Parameters.Select(p => $"{p.Name}: {p.Value}"));

        return $"{decl} => this.{setting.GetInfo.Method}({args});";
    }
}
