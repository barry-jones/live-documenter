
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class FieldRVAMetadataTableRowTests
    {
        [Test]
        public void FieldRVA_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00, 0x00, 0x00,
                0x01, 0x00
            };

            FieldRVAMetadataTableRow row = new FieldRVAMetadataTableRow(contents, 0, 2);

            Assert.AreEqual(1, row.RVA);
            Assert.AreEqual(1, row.Field.Value);
        }

        [Test]
        public void FieldRVA_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[10];

            FieldRVAMetadataTableRow row = new FieldRVAMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
