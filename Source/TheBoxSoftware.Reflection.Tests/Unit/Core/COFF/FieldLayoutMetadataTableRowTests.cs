
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class FieldLayoutMetadataTableRowTests
    {
        [Test]
        public void FieldLayout_WhenCreated_ReadFieldsCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00, 0x00, 0x00,
                0x01, 0x00
            };
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            FieldLayoutMetadataTableRow row = new FieldLayoutMetadataTableRow(contents, 0, indexDetails);

            Assert.AreEqual(1, row.Offset);
            Assert.AreEqual(1, row.Field.Value);
        }

        [Test]
        public void FieldLayout_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[10];
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            FieldLayoutMetadataTableRow row = new FieldLayoutMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
