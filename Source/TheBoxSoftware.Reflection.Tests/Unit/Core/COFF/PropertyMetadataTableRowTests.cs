
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class PropertyMetadataTableRowTests
    {
        [Test]
        public void Property_WhenCreated_FieldsAreReadCorrectly()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x00, 0x10, 
                0x01, 0x00,
                0x02, 0x00
            };
            Offset offset = 0;

            PropertyMetadataTableRow row = new PropertyMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(PropertyAttributes.HasDefault, row.Attributes);
            Assert.AreEqual(1, row.NameIndex.Value);
            Assert.AreEqual(2, row.TypeIndex);
        }

        [Test]
        public void Property_WhenCreated_OffsetIsMovedOn()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[6];
            Offset offset = 0;

            PropertyMetadataTableRow row = new PropertyMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
