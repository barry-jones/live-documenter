
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyProcessorMetadataTableRowTests
    {
        [Test]
        public void AssemblyProcessor_WhenCreated_ValueShouldAlwaysBeZero()
        {
            byte[] contents = new byte[4];
            Offset offset = 0;

            AssemblyProcessorMetadataTableRow row = new AssemblyProcessorMetadataTableRow(contents, offset);

            Assert.AreEqual(0, row.Processor);
        }

        [Test]
        public void AssemblyProcessor_WhenCreated_OffsetShouldBeMovedOn4()
        {
            byte[] contents = new byte[4];
            Offset offset = 0;

            AssemblyProcessorMetadataTableRow row = new AssemblyProcessorMetadataTableRow(contents, offset);

            Assert.AreEqual(4, offset.Current);
        }
    }
}
