namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents a list of settings.
/// </summary>
public readonly struct SettingsList
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SettingsList"/> struct
    ///     with the specified handle.
    /// </summary>
    /// <param name="handle">
    ///     The handle of the settings list.
    /// </param>
    public SettingsList(ulong handle)
    {
        Handle = handle;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SettingsList"/> struct.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_new()"/>.
    /// </remarks>
    public SettingsList()
    {
        Handle = sys.settings_list_new();
    }

    /// <summary>
    ///     Gets the handle of the settings list.
    /// </summary>
    public ulong Handle { get; }

    /// <summary>
    ///     Gets a value indicating whether the setting is valid.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the setting's handle is not zero;
    ///     otherwise, <see langword="false"/>.
    /// </value>
    public bool IsValid => Handle != 0;

    /// <summary>
    ///     Gets the number of settings in the list.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_len(ulong)"/>.
    /// </remarks>
    public ulong Count => sys.settings_list_len(Handle);

    /// <summary>
    ///     Gets the setting at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the setting to get.
    /// </param>
    /// <returns>
    ///     The setting at the specified index.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_get(ulong, ulong)"/>.
    /// </remarks>
    public Setting this[ulong index] => new(sys.settings_list_get(Handle, index));

    /// <summary>
    ///     Adds a setting to the list.
    /// </summary>
    /// <param name="value">
    ///     The setting to add.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_push(ulong, ulong)"/>.
    /// </remarks>
    public void Add(Setting value)
    {
        sys.settings_list_push(Handle, value.Handle);
    }

    /// <summary>
    ///     Inserts a setting at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index at which to insert the setting.
    /// </param>
    /// <param name="value">
    ///     The setting to insert.
    /// </param>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_insert(ulong, ulong, ulong)"/>.
    /// </remarks>
    public void Insert(ulong index, Setting value)
    {
        sys.settings_list_insert(Handle, index, value.Handle);
    }

    /// <summary>
    ///     Copies the settings list.
    /// </summary>
    /// <returns>
    ///     A copy of the settings list.
    /// </returns>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_copy(ulong)"/>.
    /// </remarks>
    public SettingsList Copy()
    {
        return new(sys.settings_list_copy(Handle));
    }

    /// <summary>
    ///     Frees the settings list.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="sys.settings_list_free(ulong)"/>.
    /// </remarks>
    public void Free()
    {
        sys.settings_list_free(Handle);
    }
}
