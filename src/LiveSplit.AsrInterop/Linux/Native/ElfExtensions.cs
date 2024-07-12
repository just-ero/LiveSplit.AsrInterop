namespace LiveSplit.AsrInterop.Linux.Native;

internal enum ElfExtension : byte
{
    None,

    HpUx,
    NetBsd,
    Linux,
    Solaris,
    Aix,
    Irix,
    FreeBsd,
    Tru64,
    Modesto,
    OpenBsd,
    OpenVms,
    NonStopKernel,

    ArchitectureSpecific = 64
}
