
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
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
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            AssemblyRefOSMetadataTableRow row = new AssemblyRefOSMetadataTableRow(content, offset, indexDetails);

            Assert.AreEqual(0, row.OSPlatformID);
            Assert.AreEqual(0, row.OSMajorVersion);
            Assert.AreEqual(0, row.OSMinorVersion);
            Assert.AreEqual(0, row.AssemblyRef.Value);
        }

        [TestCase(2, 14)]
        [TestCase(4, 16)]
        public void AssemblyRefOS_WhenCreated_OffsetIsMovedOn(byte sizeOfIndex, int expected)
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(sizeOfIndex);
            Offset offset = 0;
            byte[] content = new byte[20];

            AssemblyRefOSMetadataTableRow row = new AssemblyRefOSMetadataTableRow(content, offset, indexDetails);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
