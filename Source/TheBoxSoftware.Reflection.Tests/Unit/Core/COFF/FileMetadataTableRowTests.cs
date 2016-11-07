
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class FileMetadataTableRowTests
    {
        [Test]
        public void File_WhenCreated_ReadsFieldsCorrectly()
        {
            byte[] contents = new byte[] {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00
            };

            FileMetadataTableRow row = new FileMetadataTableRow(contents, 0, 2, 2);

            Assert.AreEqual(FileAttributes.ContainsMetadata, row.Flags);
            Assert.AreEqual(0, row.Name.Value);
            Assert.AreEqual(0, row.HashValue);
        }

        [Test]
        public void File_WhenCreated_OffsetIsMovedOn()
        {
            Offset offset = 0;
            byte[] contents = new byte[10];

            FileMetadataTableRow row = new FileMetadataTableRow(contents, offset, 2, 2);

            Assert.AreEqual(8, offset.Current);
        }
    }
}
