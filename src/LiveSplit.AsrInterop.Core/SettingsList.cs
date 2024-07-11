namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Provides access to marshalled <see href="https://github.com/LiveSplit/asr">asr</see> settings list functions.
/// </summary>
public static unsafe class SettingsList
{
    /// <summary>
    ///     Creates a new list of settings.
    /// </summary>
    /// <remarks>
    ///     The caller is responsible for freeing the returned list using <see cref="Free"/>.
    /// </remarks>
    public static USettingsList New()
    {
        return sys.settings_list_new();
    }

    /// <summary>
    ///     Frees the handle to the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list to free.
    /// </param>
    public static void Free(USettingsList list)
    {
        sys.settings_list_free(list);
    }

    /// <summary>
    ///     Copies the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list to copy.
    /// </param>
    /// <returns>
    ///     A copy of the list.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned list using <see cref="Free"/>.
    /// </remarks>
    public static USettingsList Copy(USettingsList list)
    {
        return sys.settings_list_copy(list);
    }

    /// <summary>
    ///     Gets the length of the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list of which to get the length.
    /// </param>
    /// <returns>
    ///     The length of the list.
    /// </returns>
    public static ulong Len(USettingsList list)
    {
        return sys.settings_list_len(list);
    }

    /// <summary>
    ///     Gets the value at the specified <paramref name="index"/> in the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list which contains the value.
    /// </param>
    /// <param name="index">
    ///     The index of the value.
    /// </param>
    /// <returns>
    ///     The value at the specified index when the method succeeds;
    ///     otherwise, <see cref="USettingValue.None"/>.
    /// </returns>
    /// <remarks>
    ///     The caller is responsible for freeing the returned value using <see cref="SettingValue.Free"/>.
    /// </remarks>
    public static USettingValue Get(USettingsList list, ulong index)
    {
        return sys.settings_list_get(list, index);
    }

    /// <summary>
    ///     Adds a value to the end of the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list to which the value should be added.
    /// </param>
    /// <param name="value">
    ///     The value to add.
    /// </param>
    public static void Push(USettingsList list, USettingValue value)
    {
        sys.settings_list_push(list, value);
    }

    /// <summary>
    ///     Inserts a value at the specified <paramref name="index"/> of the provided <paramref name="list"/>.
    /// </summary>
    /// <param name="list">
    ///     The list into which the value should be inserted.
    /// </param>
    /// <param name="index">
    ///     The index at which the value should be inserted.
    /// </param>
    /// <param name="value">
    ///     The value to insert.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the value was inserted successfully;
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Insert(USettingsList list, ulong index, USettingValue value)
    {
        return sys.settings_list_insert(list, index, value) != 0;
    }
}
