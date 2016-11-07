
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    [Flags]
    public enum FileAttributes : int
    {
        ContainsMetadata = 0x0000,
        ContainsNoMetadata = 0x0001
    }
}
