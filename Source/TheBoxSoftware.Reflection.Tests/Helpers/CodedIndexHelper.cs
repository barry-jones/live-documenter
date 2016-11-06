
namespace TheBoxSoftware.Reflection.Tests.Helpers
{
    using Moq;
    using TheBoxSoftware.Reflection.Core.COFF;

    public static class CodedIndexHelper
    {
        public static ICodedIndexResolver CreateCodedIndexResolver(int indexSize)
        {
            Mock<ICodedIndexResolver> resolver = new Mock<ICodedIndexResolver>();

            CodedIndex _codedIndex = new CodedIndex();
            _codedIndex.Table = MetadataTables.Assembly;
            _codedIndex.Index = new Index(10);

            resolver.Setup(p => p.Resolve(It.IsAny<CodedIndexes>(), It.IsAny<uint>())).Returns(_codedIndex);
            resolver.Setup(p => p.GetSizeOfIndex(It.IsAny<CodedIndexes>())).Returns(indexSize);

            return resolver.Object;
        }
    }
}
