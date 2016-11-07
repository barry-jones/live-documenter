
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyRefMetadataTableRowTests
    {
        [Test]
        public void AssemblyRef_WhenConstructed_ValuesAreCorrect()
        {
            byte[] content = {
                0x04, 0x00,
                0x01, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x63, 0x03,
                0xD9, 0x03,
                0x00, 0x00,
                0x00, 0x00
            };
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            AssemblyRefMetadataTableRow row = new AssemblyRefMetadataTableRow(content, 0, indexDetails);

            Assert.AreEqual("4.1.0.0", row.GetVersion().ToString());
            Assert.AreEqual(AssemblyFlags.SideBySideCompatible, row.Flags);
            Assert.AreEqual(867, row.PublicKeyOrToken);
            Assert.AreEqual(985, row.Name.Value);
            Assert.AreEqual(0, row.Culture.Value);
            Assert.AreEqual(0, row.HashValue);
        }

        [TestCase(2, 2, 20)]
        [TestCase(4, 2, 24)]
        [TestCase(2, 4, 24)]
        [TestCase(4, 4, 28)]
        public void AssemblyRef_WhenConstructedWithIndexSizes_OffsetIsMovedOn(byte blobIndex, byte stringIndex, int expected)
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2, stringIndex, blobIndex, 2);
            Offset offset = 0;
            byte[] content = new byte[30];

            AssemblyRefMetadataTableRow row = new AssemblyRefMetadataTableRow(content, offset, indexDetails);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
