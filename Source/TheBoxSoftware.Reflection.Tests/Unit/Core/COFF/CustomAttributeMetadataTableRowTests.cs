
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Moq;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class CustomAttributeMetadataTableRowTests
    {
        private CodedIndex _codedIndex;

        [Test]
        public void CustomAttribute_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = CreateResolver();
            byte sizeOfBlobIndexes = 2;
            byte[] contents = new byte[] {
                0x00, 0x00,
                0x00, 0x00,
                0x01, 0x00
            };

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, 0, resolver, sizeOfBlobIndexes);

            Assert.AreEqual(_codedIndex, row.Parent);
            Assert.AreEqual(_codedIndex, row.Type);
            Assert.AreEqual(1, row.Value);
        }

        [Test]
        public void CustomAttribute_WhenCreated_OffsetIsMovedOn()
        {
            ICodedIndexResolver resolver = CreateResolver();
            byte sizeOfBlobIndexes = 2;
            Offset offset = 0;
            byte[] contents = new byte[10];

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, offset, resolver, sizeOfBlobIndexes);

            Assert.AreEqual(6, offset.Current);
        }

        private ICodedIndexResolver CreateResolver()
        {
            Mock<ICodedIndexResolver> resolver = new Mock<ICodedIndexResolver>();

            _codedIndex = new CodedIndex();
            _codedIndex.Table = MetadataTables.Assembly;
            _codedIndex.Index = new Index(10);

            resolver.Setup(p => p.Resolve(It.IsAny<CodedIndexes>(), It.IsAny<uint>())).Returns(_codedIndex);
            resolver.Setup(p => p.GetSizeOfIndex(It.IsAny<CodedIndexes>())).Returns(2);

            return resolver.Object;
        }
    }
}
