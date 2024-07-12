namespace LiveSplit.AsrInterop.Windows.Native;

internal struct ImageFileHeader
{
    public ImageFileMachine Machine;
    public ushort NumberOfSections;
    public uint TimeDateStamp;
    public uint PointerToSymbolTable;
    public uint NumberOfSymbols;
    public ushort SizeOfOptionalHeader;
    public ushort Characteristics;
}
