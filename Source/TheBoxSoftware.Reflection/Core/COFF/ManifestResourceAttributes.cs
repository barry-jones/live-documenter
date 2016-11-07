
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    [Flags]
    public enum ManifestResourceAttributes : int
    {
        VisibilityMask = 0x0007,
        Public = 0x0001,
        Private = 0x0002
    }
}
