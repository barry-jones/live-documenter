
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class PropertyMapMetadataTableRowTests
    {
        [Test]
        public void PropertyMap_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x02, 0x00
            };
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            Offset offset = 0;

            PropertyMapMetadataTableRow row = new PropertyMapMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(1, row.Parent.Value);
            Assert.AreEqual(2, row.PropertyList.Value);
        }

        [Test]
        public void PropertyMap_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[4];
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            PropertyMapMetadataTableRow row = new PropertyMapMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
