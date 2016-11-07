
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public interface IIndexDetails
    {
        byte GetSizeOfIndex(MetadataTables forTable);

        byte GetSizeOfStringIndex();

        byte GetSizeOfBlobIndex();

        byte GetSizeOfGuidIndex();
    }
}
