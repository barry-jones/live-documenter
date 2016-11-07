
namespace TheBoxSoftware.Reflection.Tests.Helpers
{
    using Moq;
    using TheBoxSoftware.Reflection.Core.COFF;

    public static class IndexHelper
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

        public static IIndexDetails CreateIndexDetails(byte indexSize)
        {
            Mock<IIndexDetails> indexDetails = new Mock<IIndexDetails>();

            indexDetails.Setup(p => p.GetSizeOfGuidIndex()).Returns(indexSize);
            indexDetails.Setup(p => p.GetSizeOfStringIndex()).Returns(indexSize);
            indexDetails.Setup(p => p.GetSizeOfBlobIndex()).Returns(indexSize);
            indexDetails.Setup(p => p.GetSizeOfIndex(It.IsAny<MetadataTables>())).Returns(indexSize);

            return indexDetails.Object;
        }

        public static IIndexDetails CreateIndexDetails(byte guidSize, byte stringSize, byte blobSize, byte metadataSize)
        {
            Mock<IIndexDetails> indexDetails = new Mock<IIndexDetails>();

            indexDetails.Setup(p => p.GetSizeOfGuidIndex()).Returns(guidSize);
            indexDetails.Setup(p => p.GetSizeOfStringIndex()).Returns(stringSize);
            indexDetails.Setup(p => p.GetSizeOfBlobIndex()).Returns(blobSize);
            indexDetails.Setup(p => p.GetSizeOfIndex(It.IsAny<MetadataTables>())).Returns(metadataSize);

            return indexDetails.Object;
        }
    }
}
