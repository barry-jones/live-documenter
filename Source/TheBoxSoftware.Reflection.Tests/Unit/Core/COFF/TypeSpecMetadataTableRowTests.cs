
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class TypeSpecMetadataTableRowTests
    {
        [Test]
        public void TypeSpec_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00
            };
            Offset offset = 0;

            TypeSpecMetadataTableRow row = new TypeSpecMetadataTableRow(2, contents, offset);

            Assert.AreEqual(1, row.Signiture.Value);
        }

        [Test]
        public void TypeSpec_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[2];
            Offset offset = 0;

            TypeSpecMetadataTableRow row = new TypeSpecMetadataTableRow(2, contents, offset);

            Assert.AreEqual(2, offset.Current);
        }
    }
}
