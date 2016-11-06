
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
            // offset is moved on 12 bytes to index 11 and ready to read next byte index 12
            Assert.AreEqual(12, offset.Current); 
        }
    }
}
