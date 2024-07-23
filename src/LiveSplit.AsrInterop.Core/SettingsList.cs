namespace LiveSplit.AsrInterop.Core;

public readonly struct SettingsList
{
    public SettingsList(ulong handle)
    {
        Handle = handle;
    }

    public SettingsList()
    {
        Handle = sys.settings_list_new();
    }

    public ulong Handle { get; }
    public bool IsValid => Handle != 0;

    public ulong Count => sys.settings_list_len(Handle);

    public Setting this[ulong index] => new(sys.settings_list_get(Handle, index));

    public void Add(Setting value)
    {
        sys.settings_list_push(Handle, value.Handle);
    }

    public void Insert(ulong index, Setting value)
    {
        sys.settings_list_insert(Handle, index, value.Handle);
    }

    public SettingsList Copy()
    {
        return new(sys.settings_list_copy(Handle));
    }

    public void Free()
    {
        sys.settings_list_free(Handle);
    }
}
