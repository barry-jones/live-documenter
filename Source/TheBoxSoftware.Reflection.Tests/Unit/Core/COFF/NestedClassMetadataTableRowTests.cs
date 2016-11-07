
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
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
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            NestedClassMetadataTableRow row = new NestedClassMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(1, row.NestedClass.Value);
            Assert.AreEqual(2, row.EnclosingClass.Value);
        }

        [Test]
        public void NestedClass_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[4];
            Offset offset = 0;
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            NestedClassMetadataTableRow row = new NestedClassMetadataTableRow(contents, offset, indexDetails);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
