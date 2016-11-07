
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using Helpers;
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
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(2);

            AssemblyRefProcessorMetadataTableRow row = new AssemblyRefProcessorMetadataTableRow(content, 0, indexDetails);

            Assert.AreEqual(0, row.Processor);
            Assert.AreEqual(0, row.AssemblyRef.Value);
        }

        [TestCase(2, 6)]
        [TestCase(4, 8)]
        public void AssemblyRefProcessor_WhenCreated_ShouldMoveOffsetOn(byte sizeOfIndex, int expected)
        {
            IIndexDetails indexDetails = IndexHelper.CreateIndexDetails(sizeOfIndex);
            byte[] content = new byte[30];
            Offset offset = 0;

            AssemblyRefProcessorMetadataTableRow row = new AssemblyRefProcessorMetadataTableRow(content, offset, indexDetails);

            Assert.AreEqual(expected, offset.Current);
        }
    }
}
