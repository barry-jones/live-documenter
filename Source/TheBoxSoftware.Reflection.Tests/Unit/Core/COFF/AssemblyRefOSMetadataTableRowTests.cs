
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyRefOSMetadataTableRowTests
    {
        [Test]
        public void AssemblYRefOS_WhenCreated_AllFieldsAreZero()
        {
            Offset offset = 0;
            byte[] content = new byte[] {
                0, 10, 0, 0,
                0, 10, 0, 0,
                0, 10, 0, 0,
                0, 20
            };

            AssemblyRefOSMetadataTableRow row = new AssemblyRefOSMetadataTableRow(content, offset, 2);

            Assert.AreEqual(0, row.OSPlatformID);
            Assert.AreEqual(0, row.OSMajorVersion);
            Assert.AreEqual(0, row.OSMinorVersion);
            Assert.AreEqual(0, row.AssemblyRef.Value);
        }

        [TestCase(2, 14)]
        [TestCase(4, 16)]
        public void AssemblyRefOS_WhenCreated_OffsetIsMovedOn(int sizeOfIndex, int expected)
        {
            Offset offset = 0;
            byte[] content = new byte[20];

            AssemblyRefOSMetadataTableRow row = new AssemblyRefOSMetadataTableRow(content, offset, sizeOfIndex);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
