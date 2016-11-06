
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyOSMetadataTableRowTests
    {
        [Test]
        public void AssemblyOS_WhenConstructed_AllFieldsShouldAlwaysBeZero()
        {
            byte[] contents = { 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0 };
            Offset offset = 0;

            AssemblyOSMetadataTableRow row = new AssemblyOSMetadataTableRow(contents, offset);

            Assert.AreEqual(0, row.OSPlatformID);
            Assert.AreEqual(0, row.OSMajorVersion);
            Assert.AreEqual(0, row.OSMinorVersion);
        }

        [Test]
        public void AssemblyOS_WhenConstructed_OffsetIsMovedBy12()
        {
            byte[] contents = new byte[12];
            Offset offset = 0;

            AssemblyOSMetadataTableRow row = new AssemblyOSMetadataTableRow(contents, offset);

            Assert.AreEqual(12, offset.Current);
        }
    }
}
