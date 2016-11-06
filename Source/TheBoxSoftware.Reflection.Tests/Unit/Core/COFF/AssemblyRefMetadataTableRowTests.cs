
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyRefMetadataTableRowTests
    {
        [Test]
        public void AssemblyRef_WhenConstructed_ValuesAreCorrect()
        {
            byte stringIndex = 2;
            byte blobIndex = 2;
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

            AssemblyRefMetadataTableRow row = new AssemblyRefMetadataTableRow(content, 0, blobIndex, stringIndex);

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
            Offset offset = 0;
            byte[] content = new byte[30];

            AssemblyRefMetadataTableRow row = new AssemblyRefMetadataTableRow(content, offset, blobIndex, stringIndex);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
