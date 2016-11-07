
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class TypeSpecMetadataTableRowTests
    {
        [Test]
        public void TypeSpec_WhenCreated_FieldsAreReadCorrectly()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[] {
                0x01, 0x00
            };
            Offset offset = 0;

            TypeSpecMetadataTableRow row = new TypeSpecMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(1, row.Signiture.Value);
        }

        [Test]
        public void TypeSpec_WhenCreated_OffsetIsMovedOn()
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);
            byte[] contents = new byte[2];
            Offset offset = 0;

            TypeSpecMetadataTableRow row = new TypeSpecMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(2, offset.Current);
        }
    }
}
