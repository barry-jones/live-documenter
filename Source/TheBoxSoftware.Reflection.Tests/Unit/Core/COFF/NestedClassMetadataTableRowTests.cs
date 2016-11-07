
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class NestedClassMetadataTableRowTests
    {
        [Test]
        public void NestedClass_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x02, 0x00
            };
            Offset offset = 0;

            NestedClassMetadataTableRow row = new NestedClassMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(1, row.NestedClass.Value);
            Assert.AreEqual(2, row.EnclosingClass.Value);
        }

        [Test]
        public void NestedClass_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[4];
            Offset offset = 0;

            NestedClassMetadataTableRow row = new NestedClassMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
