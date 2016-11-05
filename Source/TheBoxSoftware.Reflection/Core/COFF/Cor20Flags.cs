
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public enum Cor20Flags : ulong
    {
        /// <summary>The image file contains IL code only</summary>
        ILOnly = 0x00000001,
        /// <summary>The image file can only be loaded in to a 32bit process</summary>
        Bit32Required = 0x00000002,
        /// <summary>This flag is obsolete</summary>
        ILLibrary = 0x00000004,
        /// <summary>The image file is protected with a strong name signiture</summary>
        StrongNameSigned = 0x00000008,
        /// <summary>The loader and the JIT compiler are required to track debug information</summary>
        TrackedDebugData = 0x00010000
    }
}