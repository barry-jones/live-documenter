using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    [Flags]
    public enum HeapOffsetSizes : byte
    {
        StringIsLarge = 0x01,
        GuidIsLarge = 0x02,
        BlobIsLarge = 0x04
    }
}