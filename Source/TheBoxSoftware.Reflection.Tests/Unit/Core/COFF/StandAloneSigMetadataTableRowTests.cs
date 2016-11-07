
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class StandAloneSigMetadataTableRowTests
    {
        [Test]
        public void StandAloneSig_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00
            };
            Offset offset = 0;

            StandAloneSigMetadataTableRow row = new StandAloneSigMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(1, row.Signiture.Value);
        }

        [Test]
        public void StandAloneSig_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[2];
            Offset offset = 0;

            StandAloneSigMetadataTableRow row = new StandAloneSigMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(2, offset.Current);
        }
    }
}
