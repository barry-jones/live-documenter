
namespace TheBoxSoftware.Reflection.Core.PE
{
    public enum FileMagicNumbers : ushort
    {
        Bit32 = 0x010b,            // must be this if a managed file
        Bit63 = 0x020b,
        ROM = 0x0107
    }
}