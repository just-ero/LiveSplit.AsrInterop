namespace LiveSplit.AsrInterop.Native.Linux;

internal unsafe struct ElfIdent
{
    public const uint ElfSignature = 0x464C457F; // '\x7FELF'

    public uint Magic;

    public ElfClass FileClass;
    public ElfEncoding DataEncoding;
    public byte FileVersion;
    public ElfExtension OsAbiExtension;
    public byte AbiVersion;

    public fixed byte Padding[7];
}
