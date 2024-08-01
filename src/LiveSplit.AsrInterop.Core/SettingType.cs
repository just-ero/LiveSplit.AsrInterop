namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents the type of a <see cref="Setting"/>.
/// </summary>
public enum SettingType : uint
{
    /// <summary>
    ///     The setting has no type.
    /// </summary>
    /// <remarks>
    ///     This indicates an invalid setting.
    /// </remarks>
    None = 0,

    /// <summary>
    ///     The setting is a <see cref="SettingsMap"/>.
    /// </summary>
    Map = 1,

    /// <summary>
    ///     The setting is a <see cref="SettingsList"/>.
    /// </summary>
    List = 2,

    /// <summary>
    ///     The setting is a boolean.
    /// </summary>
    Bool = 3,

    /// <summary>
    ///     The setting is an integer.
    /// </summary>
    Integer = 4,

    /// <summary>
    ///     The setting is a double.
    /// </summary>
    Double = 5,

    /// <summary>
    ///     The setting is a string.
    /// </summary>
    String = 6
}
