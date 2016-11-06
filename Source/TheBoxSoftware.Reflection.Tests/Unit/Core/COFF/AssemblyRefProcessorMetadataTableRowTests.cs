
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyRefProcessorMetadataTableRowTests
    {
        [Test]
        public void AssemblyRefProcessor_WhenCreated_AllFieldsShouldBeZero()
        {
            byte[] content = new byte[] {
                0, 10, 0, 10,
                0, 10
            };

            AssemblyRefProcessorMetadataTableRow row = new AssemblyRefProcessorMetadataTableRow(content, 0, 2);

            Assert.AreEqual(0, row.Processor);
            Assert.AreEqual(0, row.AssemblyRef.Value);
        }

        [TestCase(2, 6)]
        [TestCase(4, 8)]
        public void AssemblyRefProcessor_WhenCreated_ShouldMoveOffsetOn(int sizeOfIndex, int expected)
        {
            byte[] content = new byte[30];
            Offset offset = 0;

            AssemblyRefProcessorMetadataTableRow row = new AssemblyRefProcessorMetadataTableRow(content, offset, sizeOfIndex);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
