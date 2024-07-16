namespace LiveSplit.AsrInterop.Windows.Native;

internal struct ImageNtHeaders
{
    public const uint ImageNtSignature = 0x00004550; // 'PE\0\0'

    public uint Signature;
    public ImageFileHeader FileHeader;

    // Depending on the architecture, the next field is either IMAGE_OPTIONAL_HEADER32 or IMAGE_OPTIONAL_HEADER64.
    // We don't currently need to read this field.
}
