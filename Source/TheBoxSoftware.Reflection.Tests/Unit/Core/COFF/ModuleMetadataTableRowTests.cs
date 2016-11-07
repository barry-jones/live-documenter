
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ModuleMetadataTableRowTests
    {
        [Test]
        public void Module_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x00, 0x00,
                0x04, 0x00,
                0x01, 0x00,
                0x02, 0x00,
                0x03, 0x00
            };
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            ModuleMetadataTableRow row = new ModuleMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(0, row.Generation);
            Assert.AreEqual(4, row.Name.Value);
            Assert.AreEqual(1, row.Mvid);
            Assert.AreEqual(2, row.EncId);
            Assert.AreEqual(3, row.EncBaseId);
        }

        [Test]
        public void Module_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[10];
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            ModuleMetadataTableRow row = new ModuleMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(10, offset.Current);
        }
    }
}
