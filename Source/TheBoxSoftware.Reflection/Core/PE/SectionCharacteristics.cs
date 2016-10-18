using System;

namespace TheBoxSoftware.Reflection.Core.PE
{
    [Flags]
    public enum SectionCharacteristics : uint
    {
        Reserved1                       = 0x00000000,
        Reserved2                       = 0x00000001,
        Reserved3                       = 0x00000002,
        Reserved4                       = 0x00000004,
        NoPadding                       = 0x00000008,
        Reserved5                       = 0x00000010,
        ContainsCode                    = 0x00000020,
        ContainsInitialisedData         = 0x00000040,
        ContainsUnitialisedData         = 0x00000080,
        LinkOther                       = 0x00000100,
        LinkInfo                        = 0x00000200,
        Reserved6                       = 0x00000400,
        LinkRemove                      = 0x00000800,
        LinkCOMDAT                      = 0x00001000,
        GlobalPointerRel                = 0x00008000,
        MemoryPurgable                  = 0x00020000,
        // Memory16Bit                     = 0x00020000,
        MemoryLocked                    = 0x00040000,
        MemoryPreLoad                   = 0x00080000,
        Align1Byte                      = 0x00100000,
        Align2Bytes                     = 0x00200000,
        Align4Bytes                     = 0x00300000,
        Align8Bytes                     = 0x00400000,
        Align16Bytes                    = 0x00500000,
        Align32Bytes                    = 0x00600000,
        Align64Bytes                    = 0x00700000,
        Align128Bytes                   = 0x00800000,
        Align256Bytes                   = 0x00900000,
        Align512Bytes                   = 0x00a00000,
        Align1024Bytes                  = 0x00b00000,
        Align2048Bytes                  = 0x00c00000,
        Align4096Bytes                  = 0x00d00000,
        Align8192Bytes                  = 0x00e00000,
        LinkNRelocationsOVFL            = 0x01000000,
        MemoryDiscardable               = 0x02000000,
        MemoryNotCached                 = 0x04000000,
        MemoryNotPaged                  = 0x08000000,
        MemoryShared                    = 0x10000000,
        MemoryExecute                   = 0x20000000,
        MemoryRead                      = 0x40000000,
        MemoryWrite                     = 0x80000000
    }
}
