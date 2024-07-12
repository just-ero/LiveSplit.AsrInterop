namespace LiveSplit.AsrInterop.Windows.Native;

internal struct ImageNtHeaders
{
    public const uint ImageNtSignature = 0x00004550; // 'PE\0\0'

    public uint Signature;
    public ImageFileHeader FileHeader;
}
