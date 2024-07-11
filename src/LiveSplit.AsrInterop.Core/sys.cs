#pragma warning disable CS8981 // The type name 'name' only contains lower-cased ascii characters.
#pragma warning disable IDE1006 // Naming rule violation.
#pragma warning disable SYSLIB1054 // Mark the method 'method' with 'LibraryImportAttribute' instead of 'DllImportAttribute'.

using System.Runtime.InteropServices;

namespace LiveSplit.AsrInterop.Core;

internal static unsafe partial class sys
{
    private const string ImportModule = "env";

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_get_state))]
    public static extern uint timer_get_state();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_start))]
    public static extern void timer_start();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_split))]
    public static extern void timer_split();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_reset))]
    public static extern void timer_reset();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_skip_split))]
    public static extern void timer_skip_split();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_undo_split))]
    public static extern void timer_undo_split();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_set_game_time))]
    public static extern void timer_set_game_time(
        long secs,
        int nanos);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_pause_game_time))]
    public static extern void timer_pause_game_time();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_resume_game_time))]
    public static extern void timer_resume_game_time();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(timer_set_variable))]
    public static extern void timer_set_variable(
        byte* key_ptr,
        nuint key_len,
        byte* value_ptr,
        nuint value_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_attach))]
    public static extern ulong process_attach(
        byte* name_ptr,
        nuint name_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_attach_by_pid))]
    public static extern ulong process_attach_by_pid(
        ulong pid);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_list_by_name))]
    public static extern byte process_list_by_name(
        byte* name_ptr,
        nuint name_len,
        ulong* list_ptr,
        nuint* list_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_is_open))]
    public static extern byte process_is_open(
        ulong process);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_detach))]
    public static extern void process_detach(
        ulong process);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_path))]
    public static extern byte process_get_path(
        ulong process,
        byte* buf_ptr,
        nuint* buf_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_read))]
    public static extern byte process_read(
        ulong process,
        ulong address,
        byte* buf_ptr,
        nuint buf_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_module_address))]
    public static extern ulong process_get_module_address(
        ulong process,
        byte* name_ptr,
        nuint name_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_module_size))]
    public static extern ulong process_get_module_size(
        ulong process,
        byte* name_ptr,
        nuint name_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_module_path))]
    public static extern byte process_get_module_path(
        ulong process,
        byte* name_ptr,
        nuint name_len,
        byte* buf_ptr,
        nuint* buf_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_memory_range_count))]
    public static extern ulong process_get_memory_range_count(
        ulong process);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_memory_range_address))]
    public static extern ulong process_get_memory_range_address(
        ulong process,
        ulong idx);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_memory_range_size))]
    public static extern ulong process_get_memory_range_size(
        ulong process,
        ulong idx);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(process_get_memory_range_flags))]
    public static extern ulong process_get_memory_range_flags(
        ulong process,
        ulong idx);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(runtime_set_tick_rate))]
    public static extern void runtime_set_tick_rate(
        double ticks_per_second);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(runtime_print_message))]
    public static extern void runtime_print_message(
        byte* text_ptr,
        nuint text_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(runtime_get_os))]
    public static extern byte runtime_get_os(
        byte* buf_ptr,
        nuint* buf_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(runtime_get_arch))]
    public static extern byte runtime_get_arch(
        byte* buf_ptr,
        nuint* buf_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_bool))]
    public static extern byte user_settings_add_bool(
        byte* key_ptr,
        nuint key_len,
        byte* description_ptr,
        nuint description_len,
        byte default_value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_title))]
    public static extern void user_settings_add_title(
        byte* key_ptr,
        nuint key_len,
        byte* description_ptr,
        nuint description_len,
        uint heading_level);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_choice))]
    public static extern void user_settings_add_choice(
        byte* key_ptr,
        nuint key_len,
        byte* description_ptr,
        nuint description_len,
        byte* default_option_key_ptr,
        nuint default_option_key_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_choice_option))]
    public static extern byte user_settings_add_choice_option(
        byte* key_ptr,
        nuint key_len,
        byte* option_key_ptr,
        nuint option_key_len,
        byte* option_description_ptr,
        nuint option_description_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_file_select))]
    public static extern void user_settings_add_file_select(
        byte* key_ptr,
        nuint key_len,
        byte* description_ptr,
        nuint description_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_file_select_name_filter))]
    public static extern void user_settings_add_file_select_name_filter(
        byte* key_ptr,
        nuint key_len,
        byte* description_ptr,
        nuint description_len,
        byte* pattern_ptr,
        nuint pattern_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_add_file_select_mime_filter))]
    public static extern void user_settings_add_file_select_mime_filter(
        byte* key_ptr,
        nuint key_len,
        byte* mime_type_ptr,
        nuint mime_type_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(user_settings_set_tooltip))]
    public static extern void user_settings_set_tooltip(
        byte* key_ptr,
        nuint key_len,
        byte* tooltip_ptr,
        nuint tooltip_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_new))]
    public static extern ulong settings_map_new();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_free))]
    public static extern void settings_map_free(
        ulong map);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_load))]
    public static extern ulong settings_map_load();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_store))]
    public static extern void settings_map_store(
        ulong map);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_store_if_unchanged))]
    public static extern byte settings_map_store_if_unchanged(
        ulong old_map,
        ulong new_map);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_copy))]
    public static extern ulong settings_map_copy(
        ulong map);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_insert))]
    public static extern void settings_map_insert(
        ulong map,
        byte* key_ptr,
        nuint key_len,
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_get))]
    public static extern ulong settings_map_get(
        ulong map,
        byte* key_ptr,
        nuint key_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_len))]
    public static extern ulong settings_map_len(
        ulong map);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_get_key_by_index))]
    public static extern byte settings_map_get_key_by_index(
        ulong map,
        ulong idx,
        byte* buf_ptr,
        nuint* buf_len_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_map_get_value_by_index))]
    public static extern ulong settings_map_get_value_by_index(
        ulong map,
        ulong idx);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_new))]
    public static extern ulong settings_list_new();

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_free))]
    public static extern void settings_list_free(
        ulong list);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_copy))]
    public static extern ulong settings_list_copy(
        ulong list);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_len))]
    public static extern ulong settings_list_len(
        ulong list);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_get))]
    public static extern ulong settings_list_get(
        ulong list,
        ulong idx);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_push))]
    public static extern void settings_list_push(
        ulong list,
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(settings_list_insert))]
    public static extern byte settings_list_insert(
        ulong list,
        ulong idx,
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_copy))]
    public static extern ulong setting_value_copy(
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_free))]
    public static extern void setting_value_free(
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_map))]
    public static extern ulong setting_value_new_map(
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_list))]
    public static extern ulong setting_value_new_list(
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_bool))]
    public static extern ulong setting_value_new_bool(
        byte value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_i64))]
    public static extern ulong setting_value_new_i64(
        long value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_f64))]
    public static extern ulong setting_value_new_f64(
        double value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_new_string))]
    public static extern ulong setting_value_new_string(
        byte* value_ptr,
        nuint value_len);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_type))]
    public static extern uint setting_value_get_type(
        ulong value);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_map))]
    public static extern ulong setting_value_get_map(
        ulong value,
        ulong* value_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_list))]
    public static extern ulong setting_value_get_list(
        ulong value,
        ulong* value_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_bool))]
    public static extern byte setting_value_get_bool(
        ulong value,
        byte* value_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_i64))]
    public static extern byte setting_value_get_i64(
        ulong value,
        long* value_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_f64))]
    public static extern byte setting_value_get_f64(
        ulong value,
        double* value_ptr);

    [WasmImportLinkage]
    [DllImport(ImportModule, EntryPoint = nameof(setting_value_get_string))]
    public static extern byte setting_value_get_string(
        ulong value,
        byte* buf_ptr,
        nuint* buf_len_ptr);
}
