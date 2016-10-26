
namespace TheBoxSoftware.Reflection.Core.PE
{
    /// <summary>
    /// Enumeration of all the available data directories in a MS PE/COFF file.
    /// </summary>
    public enum DataDirectories : byte
    {
        ExportDirectory = 0,
        ImportTable = 1,
        ResourceTable = 2,
        ExceptionTable = 3,
        CertificateTable = 4,
        BaseRelocationTable = 5,
        DebugData = 6,
        AchitectureData = 7,
        GlobalPointer = 8,
        TLSTable = 9,
        LoadConfigurationTable = 10,
        BoundImportTable = 11,
        ImportAddressTable = 12,
        DelayImportDescriptor = 13,
        CommonLanguageRuntimeHeader = 14,
        Reserved = 15
    }
}