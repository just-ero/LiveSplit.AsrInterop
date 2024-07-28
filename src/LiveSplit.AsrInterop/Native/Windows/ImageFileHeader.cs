namespace LiveSplit.AsrInterop.Native.Windows;

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
