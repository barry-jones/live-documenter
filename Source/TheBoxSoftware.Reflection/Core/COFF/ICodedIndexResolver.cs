
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public interface ICodedIndexResolver
    {
        CodedIndex Resolve(CodedIndexes indexType, uint value);

        int GetSizeOfIndex(CodedIndexes indexType);
    }
}