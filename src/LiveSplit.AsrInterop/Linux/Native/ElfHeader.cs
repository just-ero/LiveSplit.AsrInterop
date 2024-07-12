namespace LiveSplit.AsrInterop.Linux.Native;

internal struct ElfHeader
{
    public ElfIdent Ident;

    public ElfFileType FileType;
    public ElfMachineType Machine;
    public uint Version;
    public uint EntryPointAddress;

    public uint ProgramHeaderOffset;
    public uint SectionHeaderOffset;

    public uint ProcessorFlags;
    public ushort ElfHeaderSize;

    public ushort ProgramHeaderEntrySize;
    public ushort ProgramHeaderCount;

    public ushort SectionHeaderEntrySize;
    public ushort SectionHeaderCount;

    public ushort SectionHeaderStringIndex;
}
