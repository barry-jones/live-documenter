
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    [Flags]
    public enum AssemblyHashAlgorithms : uint
    {
        None        = 0x00000000,
        ReservedMD5 = 0x00008003,
        SHA1        = 0x00008004
    }
}
