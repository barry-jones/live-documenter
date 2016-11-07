
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ClassLayoutMetadataTableRowTests
    {
        [Test]
        public void ClassLayout_WhenCreated_ValuesAreReadCorrectly()
        {
            byte[] content = new byte[] {
                0x01, 0x00,
                0x18, 0x00, 0x00, 0x00,
                0x18, 0x00
            };
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            ClassLayoutMetadataTableRow row = new ClassLayoutMetadataTableRow(content, 0, indexDetails);

            Assert.AreEqual(1, row.PackingSize);
            Assert.AreEqual(24, row.ClassSize);
            Assert.AreEqual(24, row.Parent.Value);
        }

        [TestCase(2, 8)]
        [TestCase(4, 10)]
        public void ClassLayout_WhenCreated_OffsetIsMovedOn(byte indexSize, int expected)
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(indexSize);
            Offset offset = 0;
            byte[] content = new byte[20];

            ClassLayoutMetadataTableRow row = new ClassLayoutMetadataTableRow(content, offset, indexDetails);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
