namespace LiveSplit.AsrInterop.Native.Linux;

internal enum ElfFileType : ushort
{
    None,

    Rel,
    Exec,
    Dyn,
    Core,

    LoOs = 0xFE00,
    HiOs = 0xFEFF,

    LoProc = 0xFF00,
    HiProc = 0xFFFF
}
