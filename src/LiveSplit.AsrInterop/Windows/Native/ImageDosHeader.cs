namespace LiveSplit.AsrInterop.Windows.Native;

internal unsafe struct ImageDosHeader
{
    public const ushort ImageDosSignature = 0x5A4D;

    public ushort Magic;

    public ushort LastPageByteCount;
    public ushort PagesCount;
    public ushort RelocationsCount;
    public ushort HeaderParagraphs;
    public ushort MinExtraParagraphs;
    public ushort MaxExtraParagraphs;
    public ushort InitialStackSegment;
    public ushort InitialStackPointer;
    public ushort Checksum;
    public ushort InitialInstructionPointer;
    public ushort InitialCodeSegment;
    public ushort FileAddressOfRelocationTable;
    public ushort OverlayNumber;
    public fixed ushort Reserved[4];
    public ushort OriginalEquipmentManufacturerIdentifier;
    public ushort OriginalEquipmentManufacturerInfo;
    public fixed ushort Reserved2[10];

    public int NtHeaderAddress;
}
