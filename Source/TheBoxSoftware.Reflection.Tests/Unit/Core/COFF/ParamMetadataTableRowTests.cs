
namespace TheBoxSoftware.Reflection.Tests.Unit.Core.COFF
{
    using NUnit.Framework;
    using Reflection.Core;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ParamMetadataTableRowTests
    {
        [Test]
        public void Param_WhenCreated_FieldsAreReadCorrectly()
        {
            byte[] contents = new byte[] {
                0x01, 0x00,
                0x03, 0x00,
                0x02, 0x00
            };
            Offset offset = 0;

            ParamMetadataTableRow row = new ParamMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(ParamAttributeFlags.In, row.Flags);
            Assert.AreEqual(3, row.Sequence);
            Assert.AreEqual(2, row.Name.Value);
        }

        [Test]
        public void Param_WhenCreated_OffsetIsMovedOn()
        {
            byte[] contents = new byte[6];
            Offset offset = 0;

            ParamMetadataTableRow row = new ParamMetadataTableRow(contents, offset, 2);

            Assert.AreEqual(6, offset.Current);
        }
    }
}
