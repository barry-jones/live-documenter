
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
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

            FieldLayoutMetadataTableRow row = new FieldLayoutMetadataTableRow(contents, 0, 2);

            Assert.AreEqual(1, row.Offset);
            Assert.AreEqual(1, row.Field.Value);
        }

        [Test]
        public void FieldLayout_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[10];

            FieldLayoutMetadataTableRow row = new FieldLayoutMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
