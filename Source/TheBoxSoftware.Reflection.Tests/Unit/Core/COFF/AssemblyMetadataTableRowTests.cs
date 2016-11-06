
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyMetadataTableRowTests
    {
        [Test]
        public void AssemblyMetadata_WhenCreated_ShouldHaveCorrectValues()
        {
            byte sizeOfStringIndexes = 2;
            byte sizeOfBlobIndexes = 2;
            byte[] contents = {
                0x04, 0x80, 0x00, 0x00,
                0x01, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00,
                0x9A, 0x1D,
                0x00, 0x00
            };

            AssemblyMetadataTableRow row = new AssemblyMetadataTableRow(contents, 0, sizeOfBlobIndexes, sizeOfStringIndexes);

            Assert.AreEqual(AssemblyHashAlgorithms.SHA1, row.HashAlgId);
            Assert.AreEqual("1.0.0.0", row.GetVersion().ToString());
            Assert.AreEqual(AssemblyFlags.SideBySideCompatible, row.Flags);
            Assert.AreEqual(0, row.PublicKey);
            Assert.AreEqual(0x1D9A, row.Name.Value);
            Assert.AreEqual(0x0000, row.Culture.Value);
        }
    }
}
