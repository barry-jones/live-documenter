
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
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(2);
            byte sizeOfBlobIndexes = 2;
            byte[] contents = new byte[] {
                0x00, 0x00,
                0x00, 0x00,
                0x01, 0x00
            };

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, 0, resolver, sizeOfBlobIndexes);

            Assert.IsNotNull(row.Parent);
            Assert.IsNotNull(row.Type);
            Assert.AreEqual(1, row.Value);
        }

        [TestCase(2, 2, 6)]
        public void CustomAttribute_WhenCreated_OffsetIsMovedOn(byte blobIndexSize, int codedIndexSize, int expected)
        {
            ICodedIndexResolver resolver = CodedIndexHelper.CreateCodedIndexResolver(codedIndexSize);
            byte sizeOfBlobIndexes = blobIndexSize;
            Offset offset = 0;
            byte[] contents = new byte[10];

            CustomAttributeMetadataTableRow row = new CustomAttributeMetadataTableRow(contents, offset, resolver, sizeOfBlobIndexes);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
