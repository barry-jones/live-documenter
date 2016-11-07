
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    [Flags]
    public enum EventAttributes : short
    {
        None = 0x0000,
        SpecialName = 0x0200,
        RTSpecialName = 0x0400
    }
}
