
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class CustomAttributeMetadataTableRowTests
    {
        [Test]
        public void CustomAttribute_WhenCreated_FieldsAreReadCorrectly()
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(2);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte sizeOfBlobIndexes = 2;
            byte[] contents = new byte[] {
                0x00, 0x00,
                0x00, 0x00,
                0x01, 0x00
            };

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, 0, resolver, indexDetails);

            Assert.IsNotNull(row.Parent);
            Assert.IsNotNull(row.Type);
            Assert.AreEqual(1, row.Value);
        }

        [TestCase(2, 2, 6)]
        public void CustomAttribute_WhenCreated_OffsetIsMovedOn(byte blobIndexSize, int codedIndexSize, int expected)
        {
            ICodedIndexResolver resolver = IndexHelper.CreateCodedIndexResolver(codedIndexSize);
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2, 2, blobIndexSize, 2);
            byte sizeOfBlobIndexes = blobIndexSize;
            Offset offset = 0;
            byte[] contents = new byte[10];

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, offset, resolver, indexDetails);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
